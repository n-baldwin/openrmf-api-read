using Xunit;
using openrmf_read_api.Models;
using System;

namespace tests.Models
{
    public class NessusPatchCountTests
    {
        [Fact]
        public void Test_NewNessusPatchCountIsValid()
        {
            NessusPatchCount nessusPatchCount = new NessusPatchCount();

            // Testing
            Assert.False(nessusPatchCount == null);
        }

        [Fact]
        public void Test_NessusPatchCountWithDataIsValid()
        {
            NessusPatchCount nessusPatchCount = new NessusPatchCount();

            nessusPatchCount.totalCriticalOpen = 1;
            nessusPatchCount.totalHighOpen = 1;
            nessusPatchCount.totalMediumOpen = 1;
            nessusPatchCount.totalLowOpen = 1;
            nessusPatchCount.totalInfoOpen = 1;

            // Testing
            Assert.True(nessusPatchCount.totalCriticalOpen == 1);
            Assert.True(nessusPatchCount.totalHighOpen == 1);
            Assert.True(nessusPatchCount.totalMediumOpen == 1);
            Assert.True(nessusPatchCount.totalLowOpen == 1);
            Assert.True(nessusPatchCount.totalInfoOpen == 1);
        }
    }
}
