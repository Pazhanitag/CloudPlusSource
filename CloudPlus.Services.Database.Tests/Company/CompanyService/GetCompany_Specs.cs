using CloudPlus.Test.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.Database;
using CloudPlus.Services.Identity.User;
using FluentAssertions;
using Moq;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class GetCompany_Specs
    {
        // System Under Test
        private Database.Company.CompanyService _sut;

        [SetUp]
        public void Setup()
        {
            var companies = new List<Entities.Company>
            {
                new Entities.Company
                {
                    Id = 1,
                    Name = "First Company",
                    UniqueIdentifier = "ui1"
                },
                new Entities.Company
                {
                    Id = 2,
                    Name = "Second Company",
                    UniqueIdentifier = "ui2"
                },
                new Entities.Company
                {
                    Id = 3,
                    Name = "Third Company",
                    UniqueIdentifier = "ui3"
                },
            };

            var companiesDbSet = MockSetGenerator.CreateMockSet(companies);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Companies).Returns(companiesDbSet);

            var userServiceMock = new Mock<IUserService>();

            _sut = new Database.Company.CompanyService(dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public void When_Company_With_Company_Id_Does_Not_Exist_Should_Return_Null()
        {
            var company = _sut.GetCompany(4);
            company.Should().BeNull();
        }

        [Test]
        public void When_Company_With_Company_Id_Exists_Should_Not_Return_Null()
        {
            var company = _sut.GetCompany(1);
            company.Should().NotBeNull();
            company.Name.Should().Be("First Company");
        }

        [Test]
        public void When_Company_With_Unique_Identifier_Does_Not_Exist_Should_Return_Null()
        {
            var company = _sut.GetCompany("ui4");
            company.Should().BeNull();
        }

        [Test]
        public void When_Company_With_Unique_Identifier_Exists_Should_Not_Return_Null()
        {
            var company = _sut.GetCompany("ui2");
            company.Should().NotBeNull();
            company.Name.Should().Be("Second Company");
        }
    }
}
