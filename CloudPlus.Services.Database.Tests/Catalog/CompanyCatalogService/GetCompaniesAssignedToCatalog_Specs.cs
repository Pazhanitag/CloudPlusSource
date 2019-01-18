using System;
using System.Collections.Generic;
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

namespace CloudPlus.Services.Database.Tests.Catalog.CompanyCatalogService
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class GetCompaniesAssignedToCatalog_Specs
    {
        // System Under Test
        private Database.Catalog.CompanyCatalogService _sut;

        [SetUp]
        public void Setup()
        {
            #region Companies 
            var masterCompany = new Entities.Company
            {
                Id = 1,
                Name = "Master Company",
                Parent = null
            };

            var firstChildOfMaster = new Entities.Company
            {
                Id = 2,
                Name = "First Child of Master Company",
                Parent = masterCompany
            };

            var secondChildOfMaster = new Entities.Company
            {
                Id = 3,
                Name = "Second Child of Master Company",
                Parent = masterCompany
            };

            masterCompany.MyCompanies = new List<Entities.Company> { firstChildOfMaster, secondChildOfMaster };

            var firstChildsFirstChild = new Entities.Company
            {
                Id = 4,
                Name = "First Child's First Child",
                Parent = firstChildOfMaster
            };

            var firstChildsSecondChild = new Entities.Company
            {
                Id = 5,
                Name = "First Child's Second Child",
                Parent = firstChildOfMaster
            };

            firstChildOfMaster.MyCompanies = new List<Entities.Company> { firstChildsFirstChild, firstChildsSecondChild };

            #endregion

            #region Catalogs
            var defaultCatalog = new Entities.Catalog.Catalog
            {
                Id = 1,
                Name = "Default Catalog",
            };

            var masterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 2,
                Name = "Master's Assignable Catalog",
            };

            var firstChildsFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 3,
                Name = "First Child's First Assignable Catalog",
            };

            var firstChildsSecondAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 4,
                Name = "First Child's Second Assignable Catalog",
            };

            var secondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 5,
                Name = "Second Child's Assignable Catalog",
            };
            
            var firstChildsFirstChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 6,
                Name = "First Child's First Child's Assignable Catalog",
            };

            var firstChildsSecondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 7,
                Name = "First Child's Second Child's Assignable Catalog",
            };
            #endregion

            #region Company Catalogs
            masterCompany.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = defaultCatalog,
                    CatalogId = 1,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = masterAssignableCatalog,
                    CatalogId = 2,
                    CatalogType = CatalogType.MyCatalog,
                }
            };

            firstChildOfMaster.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = defaultCatalog,
                    CatalogId = 1,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = firstChildsFirstAssignableCatalog,
                    CatalogId = 3,
                    CatalogType = CatalogType.MyCatalog,
                },
                new CompanyCatalog
                {
                    Catalog = firstChildsSecondAssignableCatalog,
                    CatalogId = 4,
                    CatalogType = CatalogType.MyCatalog,
                }
            };

            secondChildOfMaster.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = masterAssignableCatalog,
                    CatalogId = 2,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = secondChildsAssignableCatalog,
                    CatalogId = 5,
                    CatalogType = CatalogType.MyCatalog,
                },
            };

            firstChildsFirstChild.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = firstChildsFirstAssignableCatalog,
                    CatalogId = 3,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = firstChildsFirstChildsAssignableCatalog,
                    CatalogId = 6,
                    CatalogType = CatalogType.MyCatalog,
                },
            };
            firstChildsSecondChild.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = firstChildsFirstAssignableCatalog,
                    CatalogId = 3,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = firstChildsSecondChildsAssignableCatalog,
                    CatalogId = 7,
                    CatalogType = CatalogType.MyCatalog,
                },
            };

            #endregion

            var companies = new List<Entities.Company>
            {
                masterCompany,
                firstChildOfMaster,
                firstChildsFirstChild,
                firstChildsSecondChild,
                secondChildOfMaster,
            };

            var companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Companies).Returns(companiesDbSet);

            var catalogUtilitiesMock = new Mock<ICatalogUtilities>();

            var catalogProductItemServiceMock = new Mock<ICatalogProductItemService>();

            _sut = new Database.Catalog.CompanyCatalogService(dbContextMock.Object, catalogProductItemServiceMock.Object, catalogUtilitiesMock.Object);
        }

        [Test]
        public void When_Company_Does_Not_Exist_Should_Throw_CompanyNotFoundException()
        {
            Func<Task> act = async () => { await _sut.GetCompaniesAssignedToCatalog(20, 1); };
            act.Should().Throw<CompanyNotFoundException>();
        }

        [Test]
        public void When_Catalog_Does_Not_Exist_In_CompanyCatalogs_Should_Throw_NonExistingCompanyCatalogException()
        {
            Func<Task> act = async () => { await _sut.GetCompaniesAssignedToCatalog(1, 3); };
            act.Should().Throw<NonExistingCompanyCatalogException>()
                .WithMessage("CompanyId 1, CatalogId 3");
        }

        [Test]
        public async Task When_Company_Has_Children_Should_Return_Child_Companies_With_Assigned_Catalog()
        {
            var companies = await _sut.GetCompaniesAssignedToCatalog(1, 2);

            companies.Should().HaveCount(1);
        }
    }
}
