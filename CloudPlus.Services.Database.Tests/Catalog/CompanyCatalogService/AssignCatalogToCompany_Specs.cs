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

namespace CloudPlus.Services.Database.Tests.Catalog.CompanyCatalogService
{
    [TestFixture]
    public class AssignCatalogToCompanySpecs
    {
        // System Under Test
        private Database.Catalog.CompanyCatalogService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private DbSet<CompanyCatalog> _companyCatalogsDbSet;
        private Mock<CldpDbContext> _dbContextMock;
        private Mock<ICatalogProductItemService> _catalogProductItemServiceMock;

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
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    }
                }
            };

            var masterFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 2,
                Name = "Master's First Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 11,
                        ResellerPrice = 6,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 11,
                        ResellerPrice = 6,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 11,
                        ResellerPrice = 6,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 11,
                        ResellerPrice = 6,
                    }
                }
            };

            var masterSecondAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 8,
                Name = "Master's Second Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = false,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = false,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
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
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    }
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
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    }
                }
            };

            var secondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 5,
                Name = "Second Child's Assignable Catalog",
                CatalogProductItems = new List<CatalogProductItem>
                {
                    new CatalogProductItem
                    {
                        ProductItemId = 1,
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
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
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
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
                        Available = true,
                        FixedRetailPrice = true,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 2,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 3,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    },
                    new CatalogProductItem
                    {
                        ProductItemId = 4,
                        Available = true,
                        FixedRetailPrice = false,
                        RetailPrice = 10,
                        ResellerPrice = 5,
                    }
                }
            };
            #endregion

            #region Company Catalogs

            var masterAssigned = new CompanyCatalog
            {
                Catalog = defaultCatalog,
                CatalogId = 1,
                Company = masterCompany,
                CatalogType = CatalogType.Assigned,
            };
            var masterFirstAssignable = new CompanyCatalog
            {
                Catalog = masterFirstAssignableCatalog,
                CatalogId = 2,
                Company = masterCompany,
                CatalogType = CatalogType.MyCatalog
            };
            var masterSecondAssignable = new CompanyCatalog
            {
                Catalog = masterSecondAssignableCatalog,
                CatalogId = 8,
                Company = masterCompany,
                CatalogType = CatalogType.MyCatalog
            };
            masterCompany.CompanyCatalogs = new List<CompanyCatalog>
            {
                masterAssigned,
                masterFirstAssignable,
                masterSecondAssignable
            };

            var firstChildOfMasterAssigned = new CompanyCatalog
            {
                Catalog = defaultCatalog,
                CatalogId = 1,
                Company = firstChildOfMaster,
                CatalogType = CatalogType.Assigned
            };
            var firstChildOfMasterFirstAssignable = new CompanyCatalog
            {
                Catalog = firstChildsFirstAssignableCatalog,
                CatalogId = 3,
                Company = firstChildOfMaster,
                CatalogType = CatalogType.MyCatalog
            };
            var firstChildOfMasterSecondAssignable = new CompanyCatalog
            {
                Catalog = firstChildsSecondAssignableCatalog,
                CatalogId = 4,
                Company = firstChildOfMaster,
                CatalogType = CatalogType.MyCatalog
            };
            firstChildOfMaster.CompanyCatalogs = new List<CompanyCatalog>
            {
                firstChildOfMasterAssigned,
                firstChildOfMasterFirstAssignable,
                firstChildOfMasterSecondAssignable
            };

            var secondChildOfMasterAssigned = new CompanyCatalog
            {
                Catalog = masterFirstAssignableCatalog,
                CatalogId = 2,
                Company = secondChildOfMaster,
                CatalogType = CatalogType.Assigned
            };
            var secondChildOfMasterAssignable = new CompanyCatalog
            {
                Catalog = secondChildsAssignableCatalog,
                CatalogId = 5,
                Company = secondChildOfMaster,
                CatalogType = CatalogType.MyCatalog
            };
            secondChildOfMaster.CompanyCatalogs = new List<CompanyCatalog>
            {
                secondChildOfMasterAssigned,
                secondChildOfMasterAssignable
            };

            var firstChildsFirstChildAssigned = new CompanyCatalog
            {
                Catalog = firstChildsFirstAssignableCatalog,
                CatalogId = 3,
                Company = firstChildsFirstChild,
                CatalogType = CatalogType.Assigned
            };
            var firstChildsFirstChildAssignable = new CompanyCatalog
            {
                Catalog = firstChildsFirstChildsAssignableCatalog,
                CatalogId = 6,
                Company = firstChildsFirstChild,
                CatalogType = CatalogType.MyCatalog,
            };
            firstChildsFirstChild.CompanyCatalogs = new List<CompanyCatalog>
            {
                firstChildsFirstChildAssigned,
                firstChildsFirstChildAssignable
            };

            var firstChildsSecondChildAssigned = new CompanyCatalog
            {
                Catalog = firstChildsSecondAssignableCatalog,
                CatalogId = 4,
                Company = firstChildsSecondChild,
                CatalogType = CatalogType.Assigned
            };
            var firstChildsSecondsChildAssignable = new CompanyCatalog
            {
                Catalog = firstChildsSecondChildsAssignableCatalog,
                CatalogId = 7,
                Company = firstChildsSecondChild,
                CatalogType = CatalogType.MyCatalog
            };
            firstChildsSecondChild.CompanyCatalogs = new List<CompanyCatalog>
            {
                firstChildsSecondChildAssigned,
                firstChildsSecondsChildAssignable
            };

            #endregion

            var companies = new List<Entities.Company>
            {
                masterCompany,
                firstChildOfMaster,
                firstChildsFirstChild,
                firstChildsSecondChild,
                secondChildOfMaster
            };

            var companyCatalogs = new List<CompanyCatalog>
            {
                masterAssigned,
                masterFirstAssignable,
                masterSecondAssignable,
                firstChildOfMasterAssigned,
                firstChildOfMasterFirstAssignable,
                firstChildOfMasterSecondAssignable,
                secondChildOfMasterAssigned,
                secondChildOfMasterAssignable,
                firstChildsFirstChildAssigned,
                firstChildsFirstChildAssignable,
                firstChildsSecondChildAssigned,
                firstChildsSecondsChildAssignable
            };

            _companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            _companyCatalogsDbSet = MockSetGenerator.CreateAsyncMockSet(companyCatalogs);

            _dbContextMock = new Mock<CldpDbContext>();
            _dbContextMock.Setup(c => c.Companies).Returns(_companiesDbSet);
            _dbContextMock.Setup(c => c.CompanyCatalogs).Returns(_companyCatalogsDbSet);

            var catalogUtilitiesMock = new Mock<ICatalogUtilities>();

            _catalogProductItemServiceMock = new Mock<ICatalogProductItemService>();
            _catalogProductItemServiceMock.Setup(c => c.AddProductItemToCatalog(It.IsAny<CatalogProductModel>())).Returns(Task.CompletedTask);
            _catalogProductItemServiceMock.Setup(c => c.RemoveProductItemFromCatalog(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            _catalogProductItemServiceMock.Setup(c => c.UpdateFixedRetailPrice(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

            _sut = new Database.Catalog.CompanyCatalogService(_dbContextMock.Object, _catalogProductItemServiceMock.Object, catalogUtilitiesMock.Object);
        }

        [Test]
        public void When_Parent_Company_Does_Not_Exist_Should_Throw_CompanyNotFoundException()
        {
            Func<Task> act = async () => { await _sut.AssignCatalogToCompany(20, 1, 2); };
            act.Should().Throw<CompanyNotFoundException>();
        }

        [Test]
        public void When_Company_Is_Not_Child_Of_Parent_Should_Throw_CompanyNotFoundException()
        {
            Func<Task> act = async () => { await _sut.AssignCatalogToCompany(1, 4, 2); };
            act.Should().Throw<CompanyNotFoundException>();
        }

        [Test]
        public void When_Catalog_To_Assign_Is_Not_In_Parents_Catalogs_Should_Throw_NonExistingCompanyCatalogException()
        {
            Func<Task> act = async () => { await _sut.AssignCatalogToCompany(1, 2, 3); };
            act.Should().Throw<NonExistingCompanyCatalogException>();
        }


        [Test]
        public async Task When_Company_Already_Has_Assigned_Catalog_Other_Than_Catalog_To_Assign_Should_Remove_Assigned_Catalog()
        {
            const int parentId = 1;
            const int companyId = 2;
            const int catalogId = 8;

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);
            var assignedCatalogId = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned)?.Catalog.Id;
            var companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == assignedCatalogId);

            companyCatalog.Should().NotBeNull();

            await _sut.AssignCatalogToCompany(parentId, companyId, catalogId);

            companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == assignedCatalogId);
            companyCatalog.Should().BeNull();
        }

        [Test]
        public async Task Should_Add_Default_Catalog_To_CompanyCatalogs()
        {
            const int parentId = 1;
            const int companyId = 2;
            const int catalogId = 8;

            var parent = _companiesDbSet.FirstOrDefault(c => c.Id == parentId);
            var catalogToAssignId = parent?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId)?.Catalog.Id;
            var companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == catalogToAssignId);

            companyCatalog.Should().BeNull();

            await _sut.AssignCatalogToCompany(parentId, companyId, catalogId);

            companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == catalogToAssignId);
            companyCatalog.Should().NotBeNull();
        }

        [Test]
        public async Task Should_Call_SaveChanges()
        {
            const int parentId = 1;
            const int companyId = 2;
            const int catalogId = 8;

            await _sut.AssignCatalogToCompany(parentId, companyId, catalogId);

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Should_Call_UpdateFixedRetailPrice_For_Each_CatalogProductItem_Which_Exists_In_New_Assigned_Catalog_And_Is_Available_But_Not_In_My_Catalogs()
        {
            const int parentId = 1;
            const int companyId = 2;
            const int catalogId = 8;

            await _sut.AssignCatalogToCompany(parentId, companyId, catalogId);

            var numberOfTimesToCallUpdateFixedRetailPrice = 0;

            var parentCompany = _companiesDbSet.FirstOrDefault(c => c.Id == parentId);
            var catalogToAssign = parentCompany?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);
            var company = parentCompany?.MyCompanies.FirstOrDefault(c => c.Id == companyId);

            var numOfCompaniesOwnCatalogs = company?.CompanyCatalogs.Count(cc => cc.CatalogType == CatalogType.MyCatalog);
            var numOfCatalogProductItemsInCatalogToAssign = catalogToAssign?.Catalog.CatalogProductItems.Count;

            if (numOfCatalogProductItemsInCatalogToAssign != null && numOfCompaniesOwnCatalogs != null)
            {
                numberOfTimesToCallUpdateFixedRetailPrice = numOfCompaniesOwnCatalogs.Value * numOfCatalogProductItemsInCatalogToAssign.Value;
            }

            _catalogProductItemServiceMock.Verify(x => x.UpdateFixedRetailPrice(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(numberOfTimesToCallUpdateFixedRetailPrice));
        }

        [Test]
        public async Task Should_Call_AddProductItemToCatalog_For_Each_CatalogProductItem_Which_Exists_In_New_Assigned_Catalog_And_Is_Available_But_Not_In_My_Catalogs()
        {
            const int parentId = 1;
            const int companyId = 2;
            const int catalogId = 8;

            await _sut.AssignCatalogToCompany(parentId, companyId, catalogId);

            var numberOfTimesToCallAddProductItemToCatalog = 0;

            var parentCompany = _companiesDbSet.FirstOrDefault(c => c.Id == parentId);
            var catalogToAssign = parentCompany?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);
            var company = parentCompany?.MyCompanies.FirstOrDefault(c => c.Id == companyId);

            company?.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
            {
                catalogToAssign?.Catalog.CatalogProductItems.ForEach(p =>
                {
                    var myCompanyCatalogProductItem = cc.Catalog.CatalogProductItems.FirstOrDefault(cpi => cpi.ProductItemId == p.ProductItemId);

                    if (myCompanyCatalogProductItem == null && p.Available)
                    {
                        numberOfTimesToCallAddProductItemToCatalog++;
                    }
                });
            });

            _catalogProductItemServiceMock.Verify(x => x.AddProductItemToCatalog(It.IsAny<CatalogProductModel>()), Times.Exactly(numberOfTimesToCallAddProductItemToCatalog));
        }

        [Test]
        public async Task Should_Call_RemoveProductItemFromCatalog_For_Each_CatalogProductItem_Which_Exists_In_MyCatalogs_But_Is_Not_Available_In_New_Assigned_Catalog()
        {
            const int parentId = 1;
            const int companyId = 2;
            const int catalogId = 8;

            await _sut.AssignCatalogToCompany(parentId, companyId, catalogId);

            var numberOfTimesToCallRemoveProductItemFromCatalog = 0;

            var parentCompany = _companiesDbSet.FirstOrDefault(c => c.Id == parentId);
            var catalogToAssign = parentCompany?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);
            var company = parentCompany?.MyCompanies.FirstOrDefault(c => c.Id == companyId);

            company?.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc =>
            {
                catalogToAssign?.Catalog.CatalogProductItems.ForEach(p =>
                {
                    var myCompanyCatalogProductItem = cc.Catalog.CatalogProductItems.FirstOrDefault(cpi => cpi.ProductItemId == p.ProductItemId);

                    if (myCompanyCatalogProductItem != null && !p.Available)
                    {
                        numberOfTimesToCallRemoveProductItemFromCatalog++;
                    }
                });
            });

            _catalogProductItemServiceMock.Verify(x => x.RemoveProductItemFromCatalog(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(numberOfTimesToCallRemoveProductItemFromCatalog));
        }
    }
}
