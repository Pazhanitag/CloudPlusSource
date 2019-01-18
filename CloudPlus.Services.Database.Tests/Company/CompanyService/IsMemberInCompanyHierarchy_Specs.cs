using System;
using NUnit.Framework;
using Moq;
using CloudPlus.Services.Identity.User;
using CloudPlus.Database;
using System.Collections.Generic;
using CloudPlus.Test.Helpers;
using FluentAssertions;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class IsMemberInCompanyHierarchy_Specs
    {
        // System Under Test
        private Database.Company.CompanyService _sut;

        [SetUp]
        public void Setup()
        {
            var grandchildCompany = new Entities.Company
            {
                Id = 3,
                Name = "Grand Child Company",
            };

            var childCompany = new Entities.Company
            {
                Id = 2,
                Name = "Child Company",
                MyCompanies = new List<Entities.Company> { grandchildCompany }
            };

            var parentCompany = new Entities.Company
            {
                Id = 1,
                Name = "Parent Company",
                MyCompanies = new List<Entities.Company> { childCompany }
            };

            var companies = new List<Entities.Company>
            {
                parentCompany,
                childCompany,
                grandchildCompany
            };

            var companiesDbSet = MockSetGenerator.CreateMockSet(companies);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Companies).Returns(companiesDbSet);

            var userServiceMock = new Mock<IUserService>();

            _sut = new Database.Company.CompanyService(dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public void When_Child_Company_Is_First_Child_Of_Parent_Company_Should_Return_True()
        {
            var result = _sut.IsMemberInCompanyHierarchy(1, 2);
            result.Should().BeTrue();
        }

        [Test]
        public void When_Child_Company_Is_Descendant_Of_Parent_Company_Should_Return_True()
        {
            var result = _sut.IsMemberInCompanyHierarchy(1, 3);
            result.Should().BeTrue();
        }

        [Test]
        public void When_Child_Company_Is_Not_Descendant_Or_Child_Of_Parent_Company_Should_Return_False()
        {
            var result = _sut.IsMemberInCompanyHierarchy(2, 1);
            result.Should().BeFalse();
        }

        [Test]
        public void When_Parent_Company_Does_Not_Exist_Should_Throw_Exception()
        {
            _sut.Invoking(y => y.IsMemberInCompanyHierarchy(4, 1))
                .Should().Throw<Exception>()
                .WithMessage("There is no company with id 4");
        }
    }
}
