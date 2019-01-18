using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CloudPlus.Database;
using CloudPlus.Test.Helpers;

namespace CloudPlus.Services.Database.Tests.Product.ProductService
{
    // ReSharper disable once InconsistentNaming
    public  class GetProduct_Specs
    {
        private Database.Product.ProductService _sut;

        [SetUp]
        public void Setup()
        {
            var products = new List<Entities.Catalog.Product>
            {
                new Entities.Catalog.Product
                {
                    Id = 1,
                    Name = "First product"
                },
                new Entities.Catalog.Product
                {
                    Id = 2,
                    Name = "Second product"
                }
            };

            var productDbSet = MockSetGenerator.CreateAsyncMockSet(products);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(p => p.Products).Returns(productDbSet);
            _sut = new Database.Product.ProductService(dbContextMock.Object);
        }

        [Test]
        public async Task When_Product_With_Id_Exists_Should_Return_Product()
        {
            var product = await _sut.GetProduct(1);
            product.Name.Should().Be("First product");
        }

        [Test]
        public void When_Product_With_Id_Does_Not_Exist_Should_Throw_Exception()
        {
            Func<Task> product = async () => { await _sut.GetProduct(30); };
            product.Should().Throw<Exception>().WithMessage("Could not find product with id 30");
        }
    }
}
