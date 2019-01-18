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
    public class ChangeProductAvailability_Specs
    {
        // System Under Test
        private Database.Catalog.CatalogProductItemService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private DbSet<CatalogProductItem> _catalogProductItemsDbSet;
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

            #region CatalogProductItems
            var defaultCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 1,
                Available = false
            };
            var defaultCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 1,
                Available = true
            };
            var defaultCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 1,
                Available = true
            };
            var defaultCatalogFourthProductItem = new CatalogProductItem
            {
                ProductItemId = 4,
                CatalogId = 1,
                Available = true
            };

            var masterAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 2,
                Available = true
            };
            var masterAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 2,
                Available = true
            };
            var masterAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 2,
                Available = true
            };
            var masterAssignableCatalogFourthProductItem = new CatalogProductItem
            {
                ProductItemId = 4,
                CatalogId = 2,
                Available = true
            };

            var firstChildsFirstAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 3,
                Available = true
            };
            var firstChildsFirstAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 3,
                Available = true
            };

            var firstChildsSecondAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 4,
                Available = true
            };
            var firstChildsSecondAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 4,
                Available = true
            };
            var firstChildsSecondAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 4,
                Available = true
            };

            var secondChildsFirstAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 5,
                Available = true
            };
            var secondChildsFirstAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 5,
                Available = true
            };
            var secondChildsFirstAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 5,
                Available = true
            };

            var firstChildsFirstChildsAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 6,
                Available = true
            };
            var firstChildsFirstChildsAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 6,
                Available = true
            };
            var firstChildsFirstChildsAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 6,
                Available = true
            };

            var firstChildsSecondChildsAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 7,
                Available = true
            };
            var firstChildsSecondChildsAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 7,
                Available = true
            };

            var secondChildsFirstChildsAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 8,
                Available = true
            };
            var secondChildsFirstChildsAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 8,
                Available = true
            };
            var secondChildsFirstChildsAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 8,
                Available = true
            };

            var secondChildsSecondChildsAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 9,
                Available = true
            };
            var secondChildsSecondChildsAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 9,
                Available = true
            };
            var secondChildsSecondChildsAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 9,
                Available = true
            };

            var firstGrandGrandChildOfMasterAssignableCatalogFirstProductItem = new CatalogProductItem
            {
                ProductItemId = 1,
                CatalogId = 10,
                Available = true
            };
            var firstGrandGrandChildOfMasterAssignableCatalogSecondProductItem = new CatalogProductItem
            {
                ProductItemId = 2,
                CatalogId = 10,
                Available = true
            };
            var firstGrandGrandChildOfMasterAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 10,
                Available = true
            };

            var secondGrandGrandChildOfMasterAssignableCatalogThirdProductItem = new CatalogProductItem
            {
                ProductItemId = 3,
                CatalogId = 11,
                Available = true
            };

            #endregion

            #region Catalogs
            var defaultCatalog = new Entities.Catalog.Catalog
            {
                Id = 1,
                Name = "Default Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    defaultCatalogFirstProductItem,
                    defaultCatalogSecondProductItem,
                    defaultCatalogThirdProductItem,
                    defaultCatalogFourthProductItem
                }
            };

            var masterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 2,
                Name = "Master's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    masterAssignableCatalogFirstProductItem,
                    masterAssignableCatalogSecondProductItem,
                    masterAssignableCatalogThirdProductItem,
                    masterAssignableCatalogFourthProductItem,
                }
            };

            var firstChildsFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 3,
                Name = "First Child's First Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    firstChildsFirstAssignableCatalogFirstProductItem,
                    firstChildsFirstAssignableCatalogSecondProductItem
                }
            };

            var firstChildsSecondAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 4,
                Name = "First Child's Second Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    firstChildsSecondAssignableCatalogFirstProductItem,
                    firstChildsSecondAssignableCatalogSecondProductItem,
                    firstChildsSecondAssignableCatalogThirdProductItem
                }
            };

            var secondChildsFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 5,
                Name = "Second Child's First Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    secondChildsFirstAssignableCatalogFirstProductItem,
                    secondChildsFirstAssignableCatalogSecondProductItem,
                    secondChildsFirstAssignableCatalogThirdProductItem
                }
            };

            var firstChildsFirstChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 6,
                Name = "First Child's First Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    firstChildsFirstChildsAssignableCatalogFirstProductItem,
                    firstChildsFirstChildsAssignableCatalogSecondProductItem,
                    firstChildsFirstChildsAssignableCatalogThirdProductItem
                }
            };

            var firstChildsSecondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 7,
                Name = "First Child's Second Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    firstChildsSecondChildsAssignableCatalogFirstProductItem,
                    firstChildsSecondChildsAssignableCatalogSecondProductItem
                }
            };

            var secondChildsFirstChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 8,
                Name = "Second Child's First Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    secondChildsFirstChildsAssignableCatalogFirstProductItem,
                    secondChildsFirstChildsAssignableCatalogSecondProductItem,
                    secondChildsFirstChildsAssignableCatalogThirdProductItem
                }
            };

            var secondChildsSecondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 9,
                Name = "Second Child's Second Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    secondChildsSecondChildsAssignableCatalogFirstProductItem,
                    secondChildsSecondChildsAssignableCatalogSecondProductItem,
                    secondChildsSecondChildsAssignableCatalogThirdProductItem
                }
            };

            var firstGrandGrandChildOfMasterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 10,
                Name = "First Grand Grand Child of Master's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    firstGrandGrandChildOfMasterAssignableCatalogFirstProductItem,
                    firstGrandGrandChildOfMasterAssignableCatalogSecondProductItem,
                    firstGrandGrandChildOfMasterAssignableCatalogThirdProductItem
                }
            };

            var secondGrandGrandChildOfMasterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 11,
                Name = "Second Grand Grand Child of Master's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    secondGrandGrandChildOfMasterAssignableCatalogThirdProductItem
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
                    CatalogType = CatalogType.Assigned,
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
            var catalogProductItems = new List<CatalogProductItem>
            {
                defaultCatalogFirstProductItem,
                defaultCatalogSecondProductItem,
                defaultCatalogThirdProductItem,
                defaultCatalogFourthProductItem,
                masterAssignableCatalogFirstProductItem,
                masterAssignableCatalogSecondProductItem,
                masterAssignableCatalogThirdProductItem,
                masterAssignableCatalogFourthProductItem,
                firstChildsFirstAssignableCatalogFirstProductItem,
                firstChildsFirstAssignableCatalogSecondProductItem,
                firstChildsSecondAssignableCatalogFirstProductItem,
                firstChildsSecondAssignableCatalogSecondProductItem,
                firstChildsSecondAssignableCatalogThirdProductItem,
                secondChildsFirstAssignableCatalogFirstProductItem,
                secondChildsFirstAssignableCatalogSecondProductItem,
                secondChildsFirstAssignableCatalogThirdProductItem,
                firstChildsFirstChildsAssignableCatalogFirstProductItem,
                firstChildsFirstChildsAssignableCatalogSecondProductItem,
                firstChildsFirstChildsAssignableCatalogThirdProductItem,
                firstChildsSecondChildsAssignableCatalogFirstProductItem,
                firstChildsSecondChildsAssignableCatalogSecondProductItem,
                secondChildsFirstChildsAssignableCatalogFirstProductItem,
                secondChildsFirstChildsAssignableCatalogSecondProductItem,
                secondChildsFirstChildsAssignableCatalogThirdProductItem,
                secondChildsSecondChildsAssignableCatalogFirstProductItem,
                secondChildsSecondChildsAssignableCatalogSecondProductItem,
                secondChildsSecondChildsAssignableCatalogThirdProductItem,
                firstGrandGrandChildOfMasterAssignableCatalogFirstProductItem,
                firstGrandGrandChildOfMasterAssignableCatalogSecondProductItem,
                firstGrandGrandChildOfMasterAssignableCatalogThirdProductItem,
                secondGrandGrandChildOfMasterAssignableCatalogThirdProductItem
            };

            _companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            _catalogProductItemsDbSet = MockSetGenerator.CreateAsyncMockSet(catalogProductItems);
            _dbContextMock = new Mock<CldpDbContext>();
            _dbContextMock.Setup(c => c.Companies).Returns(_companiesDbSet);
            _dbContextMock.Setup(c => c.CatalogProductItems).Returns(_catalogProductItemsDbSet);

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

            Func<Task> act = async () => { await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, true); };
            act.Should().Throw<CompanyNotFoundException>();
        }

        [Test]
        public void When_CompanyCatalog_With_CatalogId_Is_Not_In_CompanyCatalogs_Should_Throw_NonExistingCompanyCatalogException()
        {
            const int companyId = 1;
            const int productItemId = 1;
            const int catalogId = 3;

            Func<Task> act = async () => { await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, true); };
            act.Should().Throw<NonExistingCompanyCatalogException>();
        }

        [Test]
        public void When_ProductItem_Does_Not_Exist_In_Catalog_Should_Throw_Exception()
        {
            const int companyId = 1;
            const int productItemId = 5;
            const int catalogId = 2;

            Func<Task> act = async () => { await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, true); };
            act.Should().Throw<Exception>()
                .WithMessage("There is no product 5 in catalog 2 for company 1");
        }

        [Test]
        public async Task When_ProductAvailibility_Is_True_Should_Set_ProductAvailibility_To_True()
        {
            const int companyId = 1;
            const int productItemId = 1;
            const int catalogId = 2;

            await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, true);

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);

            var catalogProductItem = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == catalogId && cc.CatalogType == CatalogType.MyCatalog)?.Catalog
                .CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

            var productAvailibility = catalogProductItem?.Available;

            productAvailibility.Should().BeTrue();

        }

        [Test]
        public async Task When_ProductAvailibility_Is_False_Should_Set_ProductAvailibility_To_False()
        {
            const int companyId = 1;
            const int productItemId = 2;
            const int catalogId = 2;

            await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, false);

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);

            var catalogProductItem = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == catalogId && cc.CatalogType == CatalogType.MyCatalog)?.Catalog
                .CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

            var productAvailibility = catalogProductItem?.Available;

            productAvailibility.Should().BeFalse();
        }

        [Test]
        public async Task When_ProductAvailibility_Is_False_And_Company_Has_Children_Should_Remove_Product_From_Childrens_Catalogs()
        {
            const int companyId = 1;
            const int productItemId = 2;
            const int catalogId = 2;

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);
            var descendantsAffectedByUpdate = GetCompanyDescendantsAffectedByUpdate(company, catalogId);

            await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, false);

            descendantsAffectedByUpdate.ForEach(c =>
            {
                c.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
                {
                    var catalogProductItem = _catalogProductItemsDbSet.FirstOrDefault(p => p.ProductItemId == productItemId && p.CatalogId == cc.CatalogId);

                    catalogProductItem.Should().BeNull();
                });
            });
        }

        [Test]
        public async Task When_ProductAvailibility_Is_True_And_Company_Has_Children_Should_Add_Product_To_Childrens_Catalogs()
        {
            const int companyId = 1;
            const int productItemId = 4;
            const int catalogId = 2;

            await _sut.ChangeProductAvailability(companyId, catalogId, productItemId, true);

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);
            var descendantsAffectedByUpdate = GetCompanyDescendantsAffectedByUpdate(company, catalogId);

            descendantsAffectedByUpdate.ForEach(c =>
            {
                c.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
                {
                    var catalogProductItem =
                        cc.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

                    catalogProductItem.Should().NotBeNull();
                });
            });
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
