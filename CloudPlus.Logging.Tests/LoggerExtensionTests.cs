using log4net;
using NUnit.Framework;

namespace CloudPlus.Logging.Tests
{
    [TestFixture]
    public class LoggerExtensionTests
    {
        [Test]
        public void GetLogger_ReturnsLogger()
        {
            //Arrange

            //Act
            var logger = LoggerExtension.GetLogger(GetType());

            //Assert
            Assert.That(logger, Is.Not.Null);
            Assert.That(logger, Is.InstanceOf<ILog>());
        }
    }
}