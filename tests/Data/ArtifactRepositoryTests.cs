using Xunit;
using System;
using System.Threading.Tasks;
using openrmf_read_api.Data;
using openrmf_read_api.Models;
using Microsoft.Extensions.Options;

/*
 *  Do note:
 *  These tests will fail without a proper
 *  MongoDB setup locally. Refer to the README
 *  in this repo or https://github.com/Cingulara/openrmf-docs/blob/master/create-users-by-hand.md
 */

namespace tests.Data
{
    public class ArtifactRepositoryTests
    {
        private readonly ArtifactRepository _artifactRepository;

        public ArtifactRepositoryTests()
        {
            Settings settings = new Settings();
            settings.ConnectionString = "mongodb://openrmf:openrmf1234!@localhost/openrmf?authSource=admin";
            settings.Database = "openrmf";

            _artifactRepository = new ArtifactRepository(Options.Create<Settings>(settings)); 
        }

        [Fact]
        public async Task Test_ArtifactRepositoryIsValid()
        {
            // Testing
            Assert.False(_artifactRepository == null);

            // For most of these, we just need them to run.
            // If they run at all, then we know nothing happened
            // in the background with things such as configuration.
            // If they fail, however, they'll throw an error.
            await _artifactRepository.GetAllArtifacts();
            await _artifactRepository.GetArtifact("someid");
            await _artifactRepository.GetArtifact("body", DateTime.Now, 256); // This currently fails.
            await _artifactRepository.CountChecklists();
            await _artifactRepository.GetLatestArtifacts(1);
            await _artifactRepository.GetCountByType("someid");
            await _artifactRepository.GetSystemArtifacts("someid");
        }
    }
}
