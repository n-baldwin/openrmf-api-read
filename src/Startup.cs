﻿// Copyright (c) Cingulara LLC 2019 and Tutela LLC 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Prometheus;
using OpenTracing;
using OpenTracing.Util;
using Jaeger;
using Jaeger.Samplers;

using openrmf_read_api.Models;
using openrmf_read_api.Data;

namespace openrmf_read_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the database components
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Environment.GetEnvironmentVariable("MONGODBCONNECTION");
                options.Database = Environment.GetEnvironmentVariable("MONGODB");
            });
            
            // Use "OpenTracing.Contrib.NetCore" to automatically generate spans for ASP.NET Core
            services.AddSingleton<ITracer>(serviceProvider =>  
            {                
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();  
                // use the environment variables to setup the Jaeger endpoints
                var config = Jaeger.Configuration.FromEnv(loggerFactory);
                var tracer = config.GetTracer();
            
                GlobalTracer.Register(tracer);  
            
                return tracer;  
            });
            services.AddOpenTracing();

            // add repositories
            services.AddTransient<IArtifactRepository, ArtifactRepository>();
            services.AddTransient<ISystemGroupRepository, SystemGroupRepository>();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "OpenRMF Read API", Version = "v1", 
                    Description = "The Read API that goes with the OpenRMF tool",
                    Contact = new Contact
                    {
                        Name = "Dale Bingham",
                        Email = "dale.bingham@cingulara.com",
                        Url = "https://github.com/Cingulara/openrmf-api-read"
                    } });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = Environment.GetEnvironmentVariable("JWT-AUTHORITY");
                o.Audience = Environment.GetEnvironmentVariable("JWT-CLIENT");
                o.IncludeErrorDetails = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT-AUTHORITY"),
                    ValidateLifetime = true
                };

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";

                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };
            });

            // setup the RBAC for this
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("roles", "[Administrator]"));
                options.AddPolicy("Editor", policy => policy.RequireRole("roles", "[Editor]"));
                options.AddPolicy("Reader", policy => policy.RequireRole("roles", "[Reader]"));
                options.AddPolicy("Assessor", policy => policy.RequireRole("roles", "[Assessor]"));
            });

            // ********************
            // USE CORS
            // ********************
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin() 
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Custom Metrics to count requests for each endpoint and the method
            var counter = Metrics.CreateCounter("openrmf_read_api_path_counter", "Counts requests to OpenRMF endpoints", new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint" }
            });
            app.Use((context, next) =>
            {
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
            // Use the Prometheus middleware
            app.UseMetricServer();
            app.UseHttpMetrics();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenRMF Read API V1");
            });

            // ********************
            // USE CORS
            // ********************
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
