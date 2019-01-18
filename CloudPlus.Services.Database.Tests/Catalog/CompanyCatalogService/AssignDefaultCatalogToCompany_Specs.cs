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

namespace CloudPlus.Services.Database.Tests.Catalog.CompanyCatalogService
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class AssignDefaultCatalogToCompany_Specs
    {
        // System Under Test
        private Database.Catalog.CompanyCatalogService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private DbSet<CompanyCatalog> _companyCatalogsDbSet;
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

            #endregion

            #region Catalogs
            var defaultCatalog = new Entities.Catalog.Catalog
            {
                Id = 1,
                Name = "Default Catalog"
            };

            var masterAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 2,
                Name = "Master's Assignable Catalog"
            };

            var firstChildsFirstAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 3,
                Name = "First Child's First Assignable Catalog"
            };

            var firstChildsSecondAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 4,
                Name = "First Child's Second Assignable Catalog"
            };

            var secondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 5,
                Name = "Second Child's Assignable Catalog"
            };

            var firstChildsFirstChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 6,
                Name = "First Child's First Child's Assignable Catalog"
            };

            var firstChildsSecondChildsAssignableCatalog = new Entities.Catalog.Catalog
            {
                Id = 7,
                Name = "First Child's Second Child's Assignable Catalog"
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
            var masterAssignable = new CompanyCatalog
            {
                Catalog = masterAssignableCatalog,
                CatalogId = 2,
                Company = masterCompany,
                CatalogType = CatalogType.MyCatalog
            };
            masterCompany.CompanyCatalogs = new List<CompanyCatalog>
            {
                masterAssigned,
                masterAssignable
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
                CatalogType = CatalogType.MyCatalog,
                Default = true
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
                Catalog = masterAssignableCatalog,
                CatalogId = 2,
                Company = secondChildOfMaster,
                CatalogType = CatalogType.Assigned
            };
            var secondChildOfMasterAssignable = new CompanyCatalog
            {
                Catalog = secondChildsAssignableCatalog,
                CatalogId = 5,
                Company = secondChildOfMaster,
                CatalogType = CatalogType.MyCatalog,
                Default = true
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
                Default = true
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
                CatalogType = CatalogType.MyCatalog,
                Default = true
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
                masterAssignable,
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

            var catalogProductItemServiceMock = new Mock<ICatalogProductItemService>();

            _sut = new Database.Catalog.CompanyCatalogService(_dbContextMock.Object, catalogProductItemServiceMock.Object, catalogUtilitiesMock.Object);
        }

        [Test]
        public void When_Parent_Company_Does_Not_Exist_Should_Throw_CompanyNotFoundException()
        {
            Func<Task> act = async () => { await _sut.AssignDefaultCatalogToCompany(20, 1); };
            act.Should().Throw<CompanyNotFoundException>()
                .WithMessage("Parent CompanyId 20");
        }

        [Test]
        public void When_Company_Is_Not_Child_Of_Parent_Should_Throw_CompanyNotFoundException()
        {
            Func<Task> act = async () => { await _sut.AssignDefaultCatalogToCompany(1, 4); };
            act.Should().Throw<CompanyNotFoundException>()
                .WithMessage("CompanyId 4");
        }

        [Test]
        public void When_Parent_Company_Has_No_Default_Catalog_Should_NonExistingCompanyCatalogException()
        {
            Func<Task> act = async () => { await _sut.AssignDefaultCatalogToCompany(1, 2); };
            act.Should().Throw<NonExistingCompanyCatalogException>();
        }

        [Test]
        public async Task When_Company_Already_Has_Assigned_Catalog_Other_Than_Default_Should_Remove_That_Catalog()
        {
            const int parentId = 2;
            const int companyId = 5;

            var company = _companiesDbSet.FirstOrDefault(c => c.Id == companyId);
            var assignedCatalogId = company?.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned)?.Catalog.Id;
            var companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == assignedCatalogId);

            companyCatalog.Should().NotBeNull();

            await _sut.AssignDefaultCatalogToCompany(parentId, companyId);

            companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == assignedCatalogId);
            companyCatalog.Should().BeNull();
        }

        [Test]
        public async Task Should_Add_Default_Catalog_To_CompanyCatalogs()
        {
            const int parentId = 2;
            const int companyId = 5;

            var parent = _companiesDbSet.FirstOrDefault(c => c.Id == parentId);
            var defaultCatalogId = parent?.CompanyCatalogs.FirstOrDefault(cc => cc.Default)?.Catalog.Id;
            var companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == defaultCatalogId);

            companyCatalog.Should().BeNull();

            await _sut.AssignDefaultCatalogToCompany(parentId, companyId);

            companyCatalog = _companyCatalogsDbSet.FirstOrDefault(cc => cc.Company.Id == companyId && cc.Catalog.Id == defaultCatalogId);
            companyCatalog.Should().NotBeNull();
        }

        [Test]
        public async Task Should_Call_SaveChanges()
        {
            const int parentId = 2;
            const int companyId = 5;

            await _sut.AssignDefaultCatalogToCompany(parentId, companyId);

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
