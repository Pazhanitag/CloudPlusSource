using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CloudPlus.Database;
using CloudPlus.Services.Identity.User;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class DeleteCompany_Specs
    {
        // System Under Test
        private Database.Company.CompanyService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private Mock<CldpDbContext> _dbContextMock;

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

            _companiesDbSet = MockSetGenerator.CreateAsyncMockSet(companies);

            _dbContextMock = new Mock<CldpDbContext>();
            _dbContextMock.Setup(c => c.Companies).Returns(_companiesDbSet);
            _dbContextMock.Setup(x => x.SaveChanges()).Verifiable();

            var userServiceMock = new Mock<IUserService>();

            _sut = new Database.Company.CompanyService(_dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public void When_Company_Gets_Deleted_Should_Decrease_Companies_Count()
        {
            _sut.DeleteCompany(1);
            var numOfCompanies = _companiesDbSet.Count();

            numOfCompanies.Should().Be(2);
        }

        [Test]
        public void When_Company_Gets_Deleted_Should_Call_Save_Changes()
        {
            _sut.DeleteCompany(1);

            _dbContextMock.Verify(x => x.SaveChanges());
        }

        [Test]
        public void When_Company_Does_Not_Exist_Should_Not_Call_Save_Changes()
        {
            _sut.DeleteCompany(4);

            _dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Never());
        }
    }
}
