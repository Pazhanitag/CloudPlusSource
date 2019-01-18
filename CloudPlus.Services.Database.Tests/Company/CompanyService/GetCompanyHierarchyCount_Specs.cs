using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Models.Identity;
using CloudPlus.Services.Identity.User;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class GetCompanyHierarchyCount_Specs
    {
        // System Under Test
        private Database.Company.CompanyService _sut;

        [SetUp]
        public void Setup()
        {
            // grandchild reseller of main company
            var mainCompanyGrandChildReseller1 = new Entities.Company
            {
                Id = 1,
                Name = "Main Company Grand Child Reseller 1",
                Type = 0,
            };
            var mainCompanyGrandChildReseller2 = new Entities.Company
            {
                Id = 2,
                Name = "Main Company Grand Child Reseller 2",
                Type = 0,
            };
            var mainCompanyGrandChildReseller3 = new Entities.Company
            {
                Id = 3,
                Name = "Main Company Grand Child Reseller 3",
                Type = 0,
            };
            // grandchild customer of main company
            var mainCompanyGrandChildCustomer1 = new Entities.Company
            {
                Id = 4,
                Name = "Main Company Grand Child Customer 1",
                Type = 1,
            };
            var mainCompanyGrandChildCustomer2 = new Entities.Company
            {
                Id = 5,
                Name = "Main Company Grand Child Customer 2",
                Type = 1,
            };
            // child resellers of main company
            var mainCompanyChildReseller1 = new Entities.Company
            {
                Id = 6,
                Name = "Main Company Child Reseller 1",
                Type = 0,
                MyCompanies = new List<Entities.Company>
                {
                    mainCompanyGrandChildReseller1,
                    mainCompanyGrandChildReseller2,
                    mainCompanyGrandChildCustomer1
                }
            };
            var mainCompanyChildReseller2 = new Entities.Company
            {
                Id = 7,
                Name = "Main Company Child Reseller 2",
                Type = 0,
                MyCompanies = new List<Entities.Company>
                {
                    mainCompanyGrandChildReseller3,
                    mainCompanyGrandChildCustomer2
                }
            };
            // child customers of main company
            var mainCompanyChildCustomer1 = new Entities.Company
            {
                Id = 8,
                Name = "Main Company Child Customer 1",
                Type = 1,
            };
            var mainCompanyChildCustomer2 = new Entities.Company
            {
                Id = 9,
                Name = "Main Company Child Customer 2",
                Type = 1,
            };
            var mainCompanyChildCustomer3 = new Entities.Company
            {
                Id = 10,
                Name = "Main Company Child Customer 3",
                Type = 1,
            };
            var mainCompanyChildCustomer4 = new Entities.Company
            {
                Id = 11,
                Name = "Main Company Child Customer 4",
                Type = 1,
            };
            // child resellers of other company
            var otherCompanyChildReseller1 = new Entities.Company
            {
                Id = 12,
                Name = "Other Company Child Reseller 1",
                Type = 0
            };
            var otherCompanyChildReseller2 = new Entities.Company
            {
                Id = 13,
                Name = "Other Company Child Reseller 2",
                Type = 0
            };
            // child customers of other company
            var otherCompanyChildCustomer1 = new Entities.Company
            {
                Id = 14,
                Name = "Other Company Child Customer 1",
                Type = 1
            };
            var otherCompanyChildCustomer2 = new Entities.Company
            {
                Id = 15,
                Name = "Other Company Child Customer 2",
                Type = 1
            };
            // main company
            var mainCompany = new Entities.Company
            {
                Id = 16,
                Name = "Main Company",
                MyCompanies = new List<Entities.Company>
                {
                    mainCompanyChildReseller1,
                    mainCompanyChildReseller2,
                    mainCompanyChildCustomer1,
                    mainCompanyChildCustomer2,
                    mainCompanyChildCustomer3,
                    mainCompanyChildCustomer4
                }
            };
            // other company
            var otherCompany = new Entities.Company
            {
                Id = 17,
                Name = "Other Company",
                MyCompanies = new List<Entities.Company>
                {
                    otherCompanyChildReseller1,
                    otherCompanyChildReseller2,
                    otherCompanyChildCustomer1,
                    otherCompanyChildCustomer2,
                }
            };

            var companies = new List<Entities.Company>
            {
                mainCompany,
                mainCompanyGrandChildReseller1,
                mainCompanyGrandChildReseller2,
                mainCompanyGrandChildReseller3,
                mainCompanyGrandChildCustomer1,
                mainCompanyGrandChildCustomer2,
                mainCompanyChildReseller1,
                mainCompanyChildReseller2,
                mainCompanyChildCustomer1,
                mainCompanyChildCustomer2,
                mainCompanyChildCustomer3,
                mainCompanyChildCustomer4,
                otherCompany,
                otherCompanyChildReseller1,
                otherCompanyChildReseller2,
                otherCompanyChildCustomer1,
                otherCompanyChildCustomer2

            };

            var companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Companies).Returns(companiesDbSet);

            var mockedUsers = new List<UserModel>
            {
                new UserModel {Id = 1, FirstName = "Fname1", LastName = "Lname1"},
                new UserModel {Id = 2, FirstName = "Fname2", LastName = "Lname2"},
                new UserModel {Id = 3, FirstName = "Fname3", LastName = "Lname3"},
                new UserModel {Id = 4, FirstName = "Fname4", LastName = "Lname4"},
                new UserModel {Id = 5, FirstName = "Fname5", LastName = "Lname5"},
            };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUsers(It.IsAny<int>())).Returns(mockedUsers);

            _sut = new Database.Company.CompanyService(dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public void When_Company_Does_Not_Exist_Should_Throw_Exception()
        {
            Func<Task> act = async () => { await _sut.GetCompanyHierarchyCount(18); };
            act.Should().Throw<Exception>()
               .WithMessage("There is no company with id 18");
        }

        [Test]
        public async Task When_Company_Has_Resellers_Should_Return_Reseller_Count()
        {
            var hierarchyCount = await _sut.GetCompanyHierarchyCount(16);
            var resellerCount = hierarchyCount.resellersCount;
            resellerCount.Should().Be(5);
        }

        [Test]
        public async Task When_Company_Has_Customers_Should_Return_Customer_Count()
        {
            var hierarchyCount = await _sut.GetCompanyHierarchyCount(16);
            var customerCount = hierarchyCount.customersCount;
            customerCount.Should().Be(6);
        }

        [Test]
        public async Task When_Company_Has_Users_Should_Return_Users_Count()
        {
            var hierarchyCount = await _sut.GetCompanyHierarchyCount(16);
            var usersCount = hierarchyCount.usersCount;
            usersCount.Should().Be(5);
        }
    }
}
