using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Catalog;
using CloudPlus.Exceptions.Catalog;
using CloudPlus.Exceptions.Company;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Catalog.CatalogProductItemService
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class ChangeRessellerPrice_Specs
    {
        // System Under Test
        private Database.Catalog.CatalogProductItemService _sut;

        private Mock<CldpDbContext> _dbContextMock;
        private DbSet<Entities.Company> _companiesDbSet;

        [SetUp]
        public void Setup()
        {
            // companies 
            var firstCompany = new Entities.Company
            {
                Id = 1,
                Name = "Master Company",
            };

            var secondCompany = new Entities.Company
            {
                Id = 2,
                Name = "Second Company",
            };

            var thirdCompany = new Entities.Company
            {
                Id = 3,
                Name = "Third Company",
            };

            // catalogs

            var firstCatalog = new Entities.Catalog.Catalog
            {
                Id = 1,
                Name = "First Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ResellerPrice = 10
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ResellerPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ResellerPrice = 30
                    }
                }
            };

            var secondCatalog = new Entities.Catalog.Catalog
            {
                Id = 2,
                Name = "Second Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ResellerPrice = 5
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ResellerPrice = 10
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ResellerPrice = 15
                    }
                }
            };

            var thirdCatalog = new Entities.Catalog.Catalog
            {
                Id = 3,
                Name = "Third Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ResellerPrice = 15
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ResellerPrice = 30
                    },
                }
            };

            // company catalogs

            firstCompany.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = firstCatalog,
                    CatalogId = 1,
                },
                new CompanyCatalog
                {
                    Catalog = secondCatalog,
                    CatalogId = 2,
                },
                new CompanyCatalog
                {
                    Catalog = thirdCatalog,
                    CatalogId = 3,
                }

            };

            var companies = new List<Entities.Company>
            {
                firstCompany,
                secondCompany,
                thirdCompany,
            };

            _companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            _dbContextMock = new Mock<CldpDbContext>();
            _dbContextMock.Setup(c => c.Companies).Returns(_companiesDbSet);

            var catalogUtilitiesMock = new Mock<ICatalogUtilities>();

            _sut = new Database.Catalog.CatalogProductItemService(_dbContextMock.Object, catalogUtilitiesMock.Object);
        }

        [Test]
        public void When_Company_Does_Not_Exist_Should_Throw_CompanyNotFoundException()
        {
            Func<Task> act = async () => { await _sut.ChangeResellerPrice(4, 1, 1, 1); };
            act.Should().Throw<CompanyNotFoundException>();
        }


        [Test]
        public void When_CompanyCatalog_Does_Not_Exist_In_CompanyCatalogs_Should_Throw_NonExistingCompanyCatalogException()
        {
            Func<Task> act = async () => { await _sut.ChangeResellerPrice(1, 1, 4, 1); };
            act.Should().Throw<NonExistingCompanyCatalogException>();
        }

        [Test]
        public void When_CatalogProductItem_Does_Not_Exist_In_Selected_CompanyCatalogs_Should_Throw_NullReferenceException()
        {
            Func<Task> act = async () => { await _sut.ChangeResellerPrice(1, 3, 3, 1); };
            act.Should().Throw<NullReferenceException>()
                .WithMessage("No product with id 3 in catalog 3");
        }

        [Test]
        public async Task Should_Set_ResellerPrice_To_NewResellerPrice()
        {
            const int companyId = 1;
            const int productItemId = 1;
            const int catalogId = 1;
            const int newResellerPrice = 20;

            await _sut.ChangeResellerPrice(companyId, productItemId, catalogId, newResellerPrice);

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);

            var catalogProductItem = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == catalogId)?.Catalog
                .CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

            var resellerPrice = catalogProductItem?.ResellerPrice;

            resellerPrice.Should().Be(newResellerPrice);
        }

        [Test]
        public async Task Should_Call_SaveChanges()
        {
            const int companyId = 1;
            const int productItemId = 1;
            const int catalogId = 1;
            const int newResellerPrice = 20;

            await _sut.ChangeResellerPrice(companyId, productItemId, catalogId, newResellerPrice);

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
