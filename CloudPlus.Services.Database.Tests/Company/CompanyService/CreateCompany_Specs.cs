using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Models.Company;
using CloudPlus.Models.Domain;
using CloudPlus.Services.Identity.User;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Company.CompanyService
{
    [TestFixture]
    public class CreateCompany_Specs
    {
        // System Under Test
        private Database.Company.CompanyService _sut;

        private DbSet<Entities.Company> _companiesDbSet;
        private DbSet<Entities.Domain> _domainsDbSet;
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

            var domains = new List<Entities.Domain>();
            _domainsDbSet = MockSetGenerator.CreateAsyncMockSet(domains);

            _dbContextMock = new Mock<CldpDbContext>();
            _dbContextMock.Setup(c => c.Companies).Returns(_companiesDbSet);
            _dbContextMock.Setup(c => c.Domains).Returns(_domainsDbSet);
            _dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1).Verifiable();

            var userServiceMock = new Mock<IUserService>();

            _sut = new Database.Company.CompanyService(_dbContextMock.Object, userServiceMock.Object);
        }

        [Test]
        public async Task When_Company_Gets_Created_Should_Increase_Companies_Count()
        {
            var newCompany = new CompanyModel
            {
                Id = 4,
                Name = "New Company",
                Domains = new List<DomainModel>
                {
                    new DomainModel
                    {
                        Id = 1,
                        Name = "New Domain"
                    }
                } 
            };

            await _sut.CreateCompany(newCompany);
            var numOfCompanies = _companiesDbSet.Count();

            numOfCompanies.Should().Be(4);
        }

        [Test]
        public async Task When_Company_Gets_Created_Should_Call_Save_Changes()
        {
            var newCompany = new CompanyModel
            {
                Id = 4,
                Name = "New Company",
                Domains = new List<DomainModel>
                {
                    new DomainModel
                    {
                        Id = 1,
                        Name = "New Domain"
                    }
                }
            };

            await _sut.CreateCompany(newCompany);

            _dbContextMock.Verify(x => x.SaveChangesAsync());
        }

        [Test]
        public async Task When_Company_Gets_Created_Should_Return_Created_Company()
        {
            var newCompany = new CompanyModel
            {
                Id = 4,
                Name = "New Company",
                Domains = new List<DomainModel>
                {
                    new DomainModel
                    {
                        Id = 1,
                        Name = "New Domain"
                    }
                }
            };

            var createdCompany = await _sut.CreateCompany(newCompany);

            createdCompany.Should().NotBeNull();
            createdCompany.Name.Should().Be("New Company");
        }

        [Test]
        public async Task When_Company_Gets_Created_With_Domains_Should_Increase_Domains_Count()
        {
            var newCompany = new CompanyModel
            {
                Id = 4,
                Name = "New Company",
                Domains = new List<DomainModel>
                {
                    new DomainModel
                    {
                        Id = 1,
                        Name = "New Domain 1"
                    },
                    new DomainModel
                    {
                        Id = 2,
                        Name = "New Domain 2"
                    },
                }
            };

            await _sut.CreateCompany(newCompany);
            var numOfDomains = _domainsDbSet.Count();

            numOfDomains.Should().Be(2);
        }

        [Test]
        public void When_Company_Is_Null_Should_Throw_ArgumentNullExcpetion()
        {
            Func<Task> act = async () => { await _sut.CreateCompany(null); };
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
