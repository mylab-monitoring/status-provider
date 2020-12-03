using MyLab.StatusProvider;
using Xunit;

namespace UnitTests
{
    public class RequestCheckerBehavior
    {
        [Theory]
        [InlineData("/status")]
        [InlineData("status")]
        public void ShouldDetectBasePath(string initialBasePath)
        {
            //Arrange
            var detector = new StatusRequestDetector(initialBasePath);

            //Act
            var relPath = detector.DetectAndGetRelatedPath("GET", "/status");

            //Assert
            Assert.Equal("", relPath);
        }

        [Theory]
        [InlineData("/status")]
        [InlineData("status")]
        public void ShouldDetectSubPath(string initialBasePath)
        {
            //Arrange
            var detector = new StatusRequestDetector(initialBasePath);

            //Act
            var relPath = detector.DetectAndGetRelatedPath("GET", "/status/foo");

            //Assert
            Assert.Equal("/foo", relPath);
        }

        [Theory]
        [InlineData("/status")]
        [InlineData("status")]
        public void ShouldNotDetectWithWrongPath(string initialBasePath)
        {
            //Arrange
            var detector = new StatusRequestDetector(initialBasePath);

            //Act
            var relPath = detector.DetectAndGetRelatedPath("GET", "/bad-status");

            //Assert
            Assert.Null(relPath);
        }

        [Theory]
        [InlineData("/status")]
        [InlineData("status")]
        public void ShouldNotDetectWithWrongMethod(string initialBasePath)
        {
            //Arrange
            var detector = new StatusRequestDetector(initialBasePath);

            //Act
            var relPath = detector.DetectAndGetRelatedPath("POST", "/status");

            //Assert
            Assert.Null(relPath);
        }
    }
}
