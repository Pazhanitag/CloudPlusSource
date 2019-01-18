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
    public class ChangeRetailPrice_Specs
    {
        // System Under Test
        private Database.Catalog.CatalogProductItemService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private Mock<CldpDbContext> _dbContextMock;

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

            var secondChildsFirstChild = new Entities.Company
            {
                Id = 6,
                Name = "Second Child's First Child",
                Parent = secondChildOfMaster
            };

            var secondChildsSecondChild = new Entities.Company
            {
                Id = 7,
                Name = "Second Child's Second Child",
                Parent = secondChildOfMaster
            };

            secondChildOfMaster.MyCompanies = new List<Entities.Company> { secondChildsFirstChild, secondChildsSecondChild };

            var firstGrandGrandChildOfMaster = new Entities.Company
            {
                Id = 8,
                Name = "Grand Child of Master's First Child",
                Parent = firstChildsFirstChild
            };

            firstChildsFirstChild.MyCompanies = new List<Entities.Company> { firstGrandGrandChildOfMaster };

            var secondGrandGrandChildOfMaster = new Entities.Company
            {
                Id = 9,
                Name = "Grand Child of Master's Second Child",
                Parent = secondChildsFirstChild
            };

            secondChildsFirstChild.MyCompanies = new List<Entities.Company> { secondGrandGrandChildOfMaster };
            #endregion

            #region Product Items
            var firstProductItem = new Entities.Catalog.ProductItem
            {
                Id = 1,
                Name = "First Product Item"
            };

            var secondProductItem = new Entities.Catalog.ProductItem
            {
                Id = 2,
                Name = "Second Product Item"
            };

            var thirdProductItem = new Entities.Catalog.ProductItem
            {
                Id = 3,
                Name = "Third Product Item"
            };

            var fourthProductItem = new Entities.Catalog.ProductItem
            {
                Id = 4,
                Name = "Fourth Product Item"
            };
            #endregion

            #region Catalogs
            var defaultCatalog = new Entities.Catalog.Catalog
            {
                Id = 1,
                Name = "Default Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 10
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 30
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        ProductItem = fourthProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 30
                    }
                }
            };

            var masterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 2,
                Name = "Master's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 5
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 15
                    }
                }
            };

            var firstChildsFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 3,
                Name = "First Child's First Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 15
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                }
            };

            var firstChildsSecondAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 4,
                Name = "First Child's Second Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 1
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 3
                    }
                }
            };

            var secondChildsFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 5,
                Name = "Second Child's First Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 3
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 9
                    }
                }
            };

            var firstChildsFirstChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 6,
                Name = "First Child's First Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 2
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 6
                    }
                }
            };

            var firstChildsSecondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 7,
                Name = "First Child's Second Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 4
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                }
            };

            var secondChildsFirstChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 8,
                Name = "Second Child's First Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 6
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 18
                    }
                }
            };

            var secondChildsSecondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 9,
                Name = "Second Child's Second Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 12
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 32
                    }
                }
            };

            var firstGrandGrandChildOfMasterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 10,
                Name = "First Grand Grand Child of Master's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        ProductItem = firstProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 11
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 33
                    }
                }
            };

            var secondGrandGrandChildOfMasterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 11,
                Name = "Second Grand Grand Child of Master's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        ProductItem = secondProductItem,
                        FixedRetailPrice = true,
                        RetailPrice = 20
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 23
                    }
                }
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
            firstGrandGrandChildOfMaster.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = firstChildsFirstChildsAssignableCatalog,
                    CatalogId = 6,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = firstGrandGrandChildOfMasterAssignableCatalog,
                    CatalogId = 10,
                    CatalogType = CatalogType.MyCatalog,
                },
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
                    Catalog = secondChildsFirstAssignableCatalog,
                    CatalogId = 5,
                    CatalogType = CatalogType.MyCatalog,
                },
            };
            secondChildsFirstChild.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = secondChildsFirstAssignableCatalog,
                    CatalogId = 5,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = secondChildsFirstChildsAssignableCatalog,
                    CatalogId = 8,
                    CatalogType = CatalogType.MyCatalog,
                },
            };
            secondChildsSecondChild.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = secondChildsFirstAssignableCatalog,
                    CatalogId = 5,
                    CatalogType = CatalogType.MyCatalog,
                },
                new CompanyCatalog
                {
                    Catalog = secondChildsSecondChildsAssignableCatalog,
                    CatalogId = 9,
                    CatalogType = CatalogType.MyCatalog,
                },
            };
            secondGrandGrandChildOfMaster.CompanyCatalogs = new List<CompanyCatalog>
            {
                new CompanyCatalog
                {
                    Catalog = secondChildsFirstChildsAssignableCatalog,
                    CatalogId = 8,
                    CatalogType = CatalogType.Assigned,
                },
                new CompanyCatalog
                {
                    Catalog = secondGrandGrandChildOfMasterAssignableCatalog,
                    CatalogId = 11,
                    CatalogType = CatalogType.MyCatalog,
                },
            };
            #endregion

            var companies = new List<Entities.Company>
            {
                masterCompany,
                firstChildOfMaster,
                firstChildsFirstChild,
                firstGrandGrandChildOfMaster,
                firstChildsSecondChild,
                secondChildOfMaster,
                secondChildsFirstChild,
                secondGrandGrandChildOfMaster,
                secondChildsSecondChild
            };

            _companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            _dbContextMock = new Mock<CldpDbContext>();
            _dbContextMock.Setup(c => c.Companies).Returns(_companiesDbSet);

            var catalogUtilitiesMock = new Mock<ICatalogUtilities>();
            catalogUtilitiesMock.Setup(x => x.CalculateResellerPrice(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(10);

            _sut = new Database.Catalog.CatalogProductItemService(_dbContextMock.Object, catalogUtilitiesMock.Object);
        }

        [Test]
        public void When_Company_Does_Not_Exist_Should_Throw_CompanyNotFoundException()
        {
            const int companyId = 20;
            const int productItemId = 1;
            const int catalogId = 1;
            const decimal newRetailPrice = 10;

            Func<Task> act = async () => { await _sut.ChangeRetailPrice(companyId, productItemId, catalogId, newRetailPrice); };
            act.Should().Throw<CompanyNotFoundException>();
        }

        [Test]
        public void When_CompanyCatalog_With_CatalogId_Is_Not_In_CompanyCatalogs_Should_Throw_NonExistingCompanyCatalogException()
        {
            const int companyId = 1;
            const int productItemId = 1;
            const int catalogId = 3;
            const decimal newRetailPrice = 10;

            Func<Task> act = async () => { await _sut.ChangeRetailPrice(companyId, productItemId, catalogId, newRetailPrice); };
            act.Should().Throw<NonExistingCompanyCatalogException>();
        }

        [Test]
        public void When_ProductItem_Does_Not_Exist_In_Catalog_Should_Throw_NullReferenceException()
        {
            const int companyId = 1;
            const int productItemId = 4;
            const int catalogId = 2;
            const decimal newRetailPrice = 10;

            Func<Task> act = async () => { await _sut.ChangeRetailPrice(companyId, productItemId, catalogId, newRetailPrice); };
            act.Should().Throw<NullReferenceException>()
                .WithMessage("No product with id 4");
        }

        [Test]
        public async Task When_FixedRetailPrice_Is_False_Should_Set_RetailPrice_In_Passed_In_Catalog()
        {
            const int companyId = 1;
            const int productItemId = 3;
            const int catalogId = 2;
            const decimal newRetailPrice = 10;

            await _sut.ChangeRetailPrice(companyId, productItemId, catalogId, newRetailPrice);

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);

            var catalogProductItem = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == catalogId)?.Catalog
                .CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

            var retailPrice = catalogProductItem?.RetailPrice;

            retailPrice.Should().Be(newRetailPrice);

        }

        [Test]
        public async Task When_FixedRetailPrice_Is_True_And_Company_Has_Children_Should_Set_RetailPrice_In_Childrens_Catalogs()
        {
            const int companyId = 1;
            const int productItemId = 2;
            const int catalogId = 2;
            const decimal newRetailPrice = 10;

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);
            var descendantsAffectedByUpdate = GetCompanyDescendantsAffectedByUpdate(company, catalogId);

            await _sut.ChangeRetailPrice(companyId, productItemId, catalogId, newRetailPrice);

            descendantsAffectedByUpdate.ForEach(c =>
            {
                c.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
                {
                    var catalogProductItem =
                        cc.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId && p.FixedRetailPrice);

                    var retailPrice = catalogProductItem?.RetailPrice;

                    retailPrice.Should().Be(newRetailPrice);
                });
            });
        }

        [Test]
        public async Task When_FixedRetailPrice_Is_True_And_Company_Has_Children_Should_Call_SaveChanges_For_Childrens_Catalogs()
        {
            const int companyId = 1;
            const int productItemId = 2;
            const int catalogId = 2;
            const decimal newRetailPrice = 10;

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);
            var descendantsAffectedByUpdate = GetCompanyDescendantsAffectedByUpdate(company, catalogId);

            await _sut.ChangeRetailPrice(companyId, productItemId, catalogId, newRetailPrice);

            //starting from 1, because it needs to call save changes for the parent
            var numOfTimesSaveChangesShouldBeCalled = 1;
            var numOfCompanyCatalogsAffectedByUpdate = 0;

            descendantsAffectedByUpdate.ForEach(c =>
            {
                c.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
                {
                    var catalogProductItem =
                        cc.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId && p.FixedRetailPrice);

                    if(catalogProductItem != null) numOfCompanyCatalogsAffectedByUpdate++;
                });
            });

            numOfTimesSaveChangesShouldBeCalled += numOfCompanyCatalogsAffectedByUpdate;

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(numOfTimesSaveChangesShouldBeCalled));
        }


        private static List<Entities.Company> GetCompanyDescendantsAffectedByUpdate(Entities.Company company, int catalogId)
        {
            var descendantsAffectedByUpdate = new List<Entities.Company>();
            var childrenWithAssignedCatalog = company.MyCompanies.Where(c =>
                c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId)).ToList();

            childrenWithAssignedCatalog.ForEach(c =>
            {
                descendantsAffectedByUpdate.Add(c);
                if (c.MyCompanies.Count <= 0) return;
                var descendants = GetAllCompanyDescendants(c);
                descendantsAffectedByUpdate.AddRange(descendants);

            });

            return descendantsAffectedByUpdate;
        }

        private static IEnumerable<Entities.Company> GetAllCompanyDescendants(Entities.Company company)
        {
            var allDescendants = new List<Entities.Company>();

            company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned)).ToList().ForEach(c =>
            {
                allDescendants.Add(c);
                if (c.MyCompanies.Count <= 0) return;
                var descendants = GetAllCompanyDescendants(c);
                allDescendants.AddRange(descendants);

            });

            return allDescendants;
        }
    }
}
