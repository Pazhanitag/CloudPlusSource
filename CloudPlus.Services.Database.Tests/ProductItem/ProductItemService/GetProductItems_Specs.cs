using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CloudPlus.Database;
using CloudPlus.Test.Helpers;

namespace CloudPlus.Services.Database.Tests.ProductItem.ProductItemService
{
    // ReSharper disable once InconsistentNaming
    public class GetProductItems_Specs
    {
        private Database.ProductItem.ProductItemService _sut;

        [SetUp]
        public void Setup()
        {
            var productItems = new List<Entities.Catalog.ProductItem>
            {
                new Entities.Catalog.ProductItem
                {
                    Id = 1,
                    Name = "Office 365 Business Essentials"
                },
                new Entities.Catalog.ProductItem
                {
                    Id = 2,
                    Name = "Office 365 Enterprise E5 (without PSTN Conferencing)"
                }
            };

            var productItemsDbSet = MockSetGenerator.CreateMockSet(productItems);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.ProductItems).Returns(productItemsDbSet);
            _sut = new Database.ProductItem.ProductItemService(dbContextMock.Object);
        }

        [Test]
        public void When_Product_Items_Exist_Should_Return_List()
        {
            var productItems = _sut.GetProductItems();
            productItems.Should().HaveCount(2);
        }
    }
}
