using CloudPlus.Resources;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Catalog.CatalogUtilities
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class CalculateResellerPrice_Specs
    {
        // System Under Test
        private Database.Catalog.CatalogUtilities _sut;

        [SetUp]
        public void SetUp()
        {
            var configurationManagerMock = new Mock<IConfigurationManager>();

            configurationManagerMock.Setup(x => x.GetByKey(It.IsAny<string>())).Returns("20");

            _sut = new Database.Catalog.CatalogUtilities(configurationManagerMock.Object);

        }

        [Test]
        public void Should_Return_Eleven()
        {
            var resellerPrice = _sut.CalculateResellerPrice(15, 10);
            resellerPrice.Should().Be(11);
        }
    }
}
