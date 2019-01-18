using System.Collections.Generic;
using System.Linq;
using CloudPlus.Database;
using CloudPlus.Models.Enums;
using CloudPlus.Services.Identity.User;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class GetCompanies_Specs
    {
        // System Under Test
        private Database.Company.CompanyService _sut;

        [SetUp]
        public void Setup()
        {
            var companies = new List<Entities.Company>
            {
                // main company
                new Entities.Company
                {
                    Id = 1,
                    Name = "Parent Company",
                    Type = 0
                },
                // reseller children of main company
                new Entities.Company
                {
                    Id = 2,
                    Name = "First Reseller Company",
                    ParentId = 1,
                    Type = 0
                },
                new Entities.Company
                {
                    Id = 3,
                    Name = "Second Reseller Company",
                    ParentId = 1,
                    Type = 0
                },
                new Entities.Company
                {
                    Id = 4,
                    Name = "Third Reseller Company",
                    ParentId = 1,
                    Type = 0
                },
                // customer children of main company
                new Entities.Company
                {
                    Id = 5,
                    Name = "First Customer Company",
                    ParentId = 1,
                    Type = 1
                },
                new Entities.Company
                {
                    Id = 6,
                    Name = "Second Customer Company",
                    ParentId = 1,
                    Type = 1
                },
                new Entities.Company
                {
                    Id = 7,
                    Name = "Third Customer Company",
                    ParentId = 1,
                    Type = 1
                },
                // reseller companies which are not children of main company
                new Entities.Company
                {
                    Id = 8,
                    Name = "Other's Reseller Company",
                    ParentId = 2,
                    Type = 0
                },
                // customer companies which are not children of main company
                new Entities.Company
                {
                    Id = 9,
                    Name = "Other's Customer Company",
                    ParentId = 2,
                    Type = 1
                },
            };

            var companiesDbSet = MockSetGenerator.CreateMockSet(companies);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Companies).Returns(companiesDbSet);

            var userServiceMock = new Mock<IUserService>();

            _sut = new Database.Company.CompanyService(dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public void When_Company_Type_Is_Reseller_Should_Return_Companies_Reseller_Children()
        {
            var companies = _sut.GetCompanies(1, CompanyType.Reseller).ToList();
            companies.Should().NotBeEmpty()
                .And.HaveCount(3)
                .And.NotContain(company => company.Type == CompanyType.Customer)
                .And.OnlyContain(company => company.ParentId == 1);
        }

        [Test]
        public void When_Company_Type_Is_Customer_Should_Return_Companies_Customer_Children()
        {
            var companies = _sut.GetCompanies(1, CompanyType.Customer).ToList();
            companies.Should().NotBeEmpty()
                .And.HaveCount(3)
                .And.NotContain(company => company.Type == CompanyType.Reseller)
                .And.OnlyContain(company => company.ParentId == 1);
        }

        [Test]
        public void When_No_Matching_Companies_Exist_Should_Return_Empty_Enumerable()
        {
            var companies = _sut.GetCompanies(3, CompanyType.Customer).ToList();
            companies.Should().BeEmpty();
        }
    }
}
