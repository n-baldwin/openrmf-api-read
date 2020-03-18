using Xunit;
using System;
using openrmf_read_api.Models;

namespace tests.Models
{
    public class NessusPatchSummaryTests
    {
        [Fact]
        public void Test_NewNessusPatchSummaryIsValid()
        {
            NessusPatchSummary nessusPatchSummary = new NessusPatchSummary();

            // Testing
            Assert.False(nessusPatchSummary == null);
        }

        [Fact]
        public void Test_NessusPatchSummaryWithDataIsValid()
        {
            NessusPatchSummary nessusPatchSummary = new NessusPatchSummary();

            nessusPatchSummary.hostname = "myHost";
            nessusPatchSummary.pluginId = "myId";
            nessusPatchSummary.pluginName = "myPlugin";
            nessusPatchSummary.family = "A family";
            nessusPatchSummary.severity = 2;
            nessusPatchSummary.hostTotal = 2;
            nessusPatchSummary.total = 2;
            nessusPatchSummary.description = "A wacky, crazy description for the ages";
            nessusPatchSummary.publicationDate = "2/2/20";
            nessusPatchSummary.pluginType = "Some sorta thingy";
            nessusPatchSummary.riskFactor = "Potentially bad for health";
            nessusPatchSummary.synopsis = "Oh no";

            // Testing
            Assert.True(nessusPatchSummary.hostname == "myHost");
            Assert.True(nessusPatchSummary.pluginId == "myId");
            Assert.True(nessusPatchSummary.pluginIdSort == "0myId");
            Assert.True(nessusPatchSummary.pluginName == "myPlugin");
            Assert.True(nessusPatchSummary.family == "A family");
            Assert.True(nessusPatchSummary.severity == 2);
            Assert.True(nessusPatchSummary.severityName == "Medium");
            Assert.True(nessusPatchSummary.hostTotal == 2);
            Assert.True(nessusPatchSummary.total == 2);
            Assert.True(nessusPatchSummary.description == "A wacky, crazy description for the ages");
            Assert.True(nessusPatchSummary.publicationDate == "2/2/20");
            Assert.True(nessusPatchSummary.pluginType == "Some sorta thingy");
            Assert.True(nessusPatchSummary.riskFactor == "Potentially bad for health");
            Assert.True(nessusPatchSummary.synopsis == "Oh no");
        }
    }
}
