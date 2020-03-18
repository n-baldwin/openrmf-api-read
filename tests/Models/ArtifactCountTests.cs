using Xunit;
using System;
using openrmf_read_api.Models; 

namespace tests.Models
{
    public class ArtifactCountTests
    {
        [Fact]
        public void Test_NewArtifactCountIsValid()
        {
            ArtifactCount artifactCount = new ArtifactCount();

            // Testing
            Assert.False(artifactCount == null);
        }

        [Fact]
        public void Test_NewArtifactCountWithDataIsValid()
        {
            ArtifactCount artifactCount = new ArtifactCount();

            artifactCount.stigType = "Google Chrome";
            artifactCount.count = 1;

            // Testing
            Assert.True(artifactCount.stigType == "Google Chrome");
            
            Assert.True(artifactCount.count == 1);
            artifactCount.count++;
            Assert.True(artifactCount.count == 2);
        }
    }
}
