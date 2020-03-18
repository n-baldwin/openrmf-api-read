using Xunit;
using System;
using openrmf_read_api.Models;

namespace tests.Models
{
    public class NessusPatchDataTests
    {
        [Fact]
        public void Test_NewNessusPatchDataIsValid()
        {
            NessusPatchData nessusPatchData = new NessusPatchData();

            // Testing
            Assert.False(nessusPatchData == null);
        }

        [Fact]
        public void Test_NessusPatchDataWithDatasIsValid()
        {
            NessusPatchData nessusPatchData = new NessusPatchData();

            nessusPatchData.reportName = "Report";
            nessusPatchData.summary.Add(new NessusPatchSummary());
            nessusPatchData.summary.Add(new NessusPatchSummary());

            // Testing
            Assert.True(nessusPatchData.reportName == "Report");
            Assert.True(nessusPatchData.summary.Count == 2);
        }
    }
}
