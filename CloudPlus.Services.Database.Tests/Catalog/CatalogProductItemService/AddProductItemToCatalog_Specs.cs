using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Catalog;
using CloudPlus.Exceptions.Catalog;
using CloudPlus.Exceptions.Company;
using CloudPlus.Models.Catalog;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Catalog.CatalogProductItemService
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class AddProductItemToCatalog_Specs
    {
        // System Under Test
        private Database.Catalog.CatalogProductItemService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private Mock<CldpDbContext> _dbContextMock;

        [SetUp]
        public void Setup()
        {
            // companies 
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

            // product items

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

            // catalogs

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
                        FixedRetailPrice = false,
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
                        FixedRetailPrice = false,
                        RetailPrice = 10
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
                        FixedRetailPrice = false,
                        RetailPrice = 30
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
                        FixedRetailPrice = false,
                        RetailPrice = 2
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
                        FixedRetailPrice = false,
                        RetailPrice = 6
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
                        FixedRetailPrice = false,
                        RetailPrice = 4
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
                        FixedRetailPrice = false,
                        RetailPrice = 8
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
                        FixedRetailPrice = false,
                        RetailPrice = 12
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
                        FixedRetailPrice = false,
                        RetailPrice = 24
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
                        FixedRetailPrice = false,
                        RetailPrice = 22
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
                        ProductItemId = 3,
                        ProductItem = thirdProductItem,
                        FixedRetailPrice = false,
                        RetailPrice = 23
                    }
                }
            };


            // company catalogs

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
            var model = new CatalogProductModel
            {
                CatalogId = 2,
                ProductItemId = 1,
                CompanyId = 20,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = true,
            };

            Func<Task> act = async () => { await _sut.AddProductItemToCatalog(model); };
            act.Should().Throw<CompanyNotFoundException>();
        }

        [Test]
        public void When_Company_Has_No_Assigned_Catalog_Should_Throw_NoAssignedCatalogException()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 2,
                ProductItemId = 1,
                CompanyId = 7,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = true,
            };

            Func<Task> act = async () => { await _sut.AddProductItemToCatalog(model); };
            act.Should().Throw<NoAssignedCatalogException>();
        }

        [Test]
        public void When_ProductItem_Is_Not_In_Assigned_Catalog_Should_Throw_NoProductInProductCatalogException()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 7,
                ProductItemId = 4,
                CompanyId = 5,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = true,
            };

            Func<Task> act = async () => { await _sut.AddProductItemToCatalog(model); };
            act.Should().Throw<NoProductInProductCatalogException>();
        }

        [Test]
        public void When_CompanyCatalog_With_CatalogId_Is_Not_In_CompanyCatalogs_Should_Throw_NonExistingCompanyCatalogException()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 7,
                ProductItemId = 3,
                CompanyId = 5,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = true,
            };

            Func<Task> act = async () => { await _sut.AddProductItemToCatalog(model); };
            act.Should().Throw<NoProductInProductCatalogException>();
        }

        [Test]
        public async Task When_CatalogProductItem_With_ProductItemId_Does_Not_Exist_In_Catalog_Should_Add_CatalogProductItem_To_Catalog()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 4,
                ProductItemId = 4,
                CompanyId = 2,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = true,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var catalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == model.CatalogId)?.Catalog;
            var catalogProductItem = catalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);

            catalogProductItem.Should().BeNull();

            await _sut.AddProductItemToCatalog(model);

            catalogProductItem = catalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);

            catalogProductItem.Should().NotBeNull();
        }

        [Test]
        public async Task When_Company_Has_Parent_Should_Set_FixedRetailPrice_To_Value_From_AssignedCatalogProductItem()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 4,
                ProductItemId = 4,
                CompanyId = 2,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var assignedCatalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned)?.Catalog;
            var expectedFixedRetailPriceValue = assignedCatalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId)?.FixedRetailPrice;

            var editedCatalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == model.CatalogId)?.Catalog;

            await _sut.AddProductItemToCatalog(model);

            var catalogProductItem = editedCatalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);
            var fixedRetailPrice = catalogProductItem?.FixedRetailPrice;

            fixedRetailPrice.Should().Be(expectedFixedRetailPriceValue);
        }

        [Test]
        public async Task When_Company_Has_No_Parent_Should_Set_FixedRetailPrice_To_Value_From_Model()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 2,
                ProductItemId = 4,
                CompanyId = 1,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var catalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == model.CatalogId)?.Catalog;

            await _sut.AddProductItemToCatalog(model);

            var catalogProductItem = catalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);
            var fixedRetailPrice = catalogProductItem?.FixedRetailPrice;

            fixedRetailPrice.Should().Be(model.FixedRetailPrice);
        }

        [Test]
        public async Task When_Company_Has_No_Parent_Should_Set_RetailPrice_To_Value_From_Model()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 2,
                ProductItemId = 4,
                CompanyId = 1,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var catalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == model.CatalogId)?.Catalog;

            await _sut.AddProductItemToCatalog(model);

            var catalogProductItem = catalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);
            var retailPrice = catalogProductItem?.RetailPrice;

            retailPrice.Should().Be(model.RetailPrice);
        }

        [Test]
        public async Task When_Company_Has_Parent_And_FixedRetailPrice_Is_True_For_Product_In_Assigned_Catalog_Should_Set_RetailPrice_To_RetailPrice_From_AssignedCatalog()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 11,
                ProductItemId = 1,
                CompanyId = 9,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var assignedCatalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned)?.Catalog;
            var expectedRetailPriceValue = assignedCatalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId)?.RetailPrice;

            var editedCatalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == model.CatalogId)?.Catalog;

            await _sut.AddProductItemToCatalog(model);

            var catalogProductItem = editedCatalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);
            var retailPrice = catalogProductItem?.RetailPrice;

            retailPrice.Should().Be(expectedRetailPriceValue);
        }

        [Test]
        public async Task When_Company_Has_Parent_And_FixedRetailPrice_Is_False_For_Product_In_Assigned_Catalog_Should_Set_RetailPrice_To_RetailPrice_From_Model()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 11,
                ProductItemId = 2,
                CompanyId = 9,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);

            var catalog = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == model.CatalogId)?.Catalog;

            await _sut.AddProductItemToCatalog(model);

            var catalogProductItem = catalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == model.ProductItemId);
            var retailPrice = catalogProductItem?.RetailPrice;

            retailPrice.Should().Be(model.RetailPrice);

        }

        [Test]
        public async Task When_Company_Has_Children_Should_Add_CatalogProductItem_To_Childrens_CompanyCatalogs()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 2,
                ProductItemId = 4,
                CompanyId = 1,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var descendantsAffectedByUpdate = GetCompanyDescendantsAffectedByUpdate(company, model.CatalogId);

            await _sut.AddProductItemToCatalog(model);

            descendantsAffectedByUpdate.ForEach(c =>
            {
                c.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
                {
                    var catalogProductItem =
                        cc.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == model.ProductItemId);

                    catalogProductItem.Should().NotBeNull();
                });
            });
        }

        [Test]
        public async Task When_Company_Has_Children_Should_Call_SaveChanges_For_All_Child_Company_Catalogs()
        {
            var model = new CatalogProductModel
            {
                CatalogId = 2,
                ProductItemId = 4,
                CompanyId = 1,
                ResellerPrice = 10,
                RetailPrice = 15,
                FixedRetailPrice = false,
            };

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == model.CompanyId);
            var descendantsAffectedByUpdate = GetCompanyDescendantsAffectedByUpdate(company, model.CatalogId);

            await _sut.AddProductItemToCatalog(model);

            //starting from 1, because it needs to call save changes for the parent
            var numOfTimesSaveChangesShouldBeCalled = 1;
            var numOfCompanyCatalogsAffectedByUpdate = 0;

            descendantsAffectedByUpdate.ForEach(c =>
            {
                c.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
                {
                    numOfCompanyCatalogsAffectedByUpdate++;
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
