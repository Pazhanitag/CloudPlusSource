using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Services.Identity.User;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class GetCompanyAsync_Specs
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
                    Name = "First Company"
                },
                new Entities.Company
                {
                    Id = 2,
                    Name = "Second Company"
                },
                new Entities.Company
                {
                    Id = 3,
                    Name = "Third Company"
                },
            };

            var companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Companies).Returns(companiesDbSet);

            var userServiceMock = new Mock<IUserService>();

            _sut = new Database.Company.CompanyService(dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public async Task When_Company_Does_Not_Exist_Should_Return_Null()
        {
            var company = await _sut.GetCompanyAsync(4);
            company.Should().BeNull();
        }

        [Test]
        public async Task When_Company_Exists_Should_Not_Return_Null()
        {
            var company = await _sut.GetCompanyAsync(1);
            company.Should().NotBeNull();
            company.Name.Should().Be("First Company");
        }
    }
}
