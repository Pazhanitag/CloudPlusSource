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
    public class UpdateCompany_Specs
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
                    Name = "First Company",
                    Domains = new List<Entities.Domain>
                    {
                        new Entities.Domain { Id = 1, Name = "First Company Primary Domain"},
                        new Entities.Domain { Id = 2, Name = "First Company Secondary Domain"}
                    }
                },
                new Entities.Company
                {
                    Id = 2,
                    Name = "Second Company",
                    Domains = new List<Entities.Domain>
                    {
                        new Entities.Domain { Id = 3, Name = "Second Company Primary Domain"},
                        new Entities.Domain { Id = 4, Name = "Second Company Secondary Domain"}
                    }
                },
                new Entities.Company
                {
                    Id = 3,
                    Name = "Third Company",
                    Domains = new List<Entities.Domain>
                    {
                        new Entities.Domain { Id = 5, Name = "Third Company Primary Domain"},
                        new Entities.Domain { Id = 6, Name = "Third Company Secondary Domain"}
                    }
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
        public void When_Model_Is_Null_Should_Throw_ArgumentNullExcpetion()
        {
            Func<Task> act = async () => { await _sut.UpdateAsync(null); };
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void When_Company_Does_Not_Exist_Should_Throw_NullReferenceException()
        {
            var companyToUpdate = new CompanyModel
            {
                Id = 4,
                Name = "Nonexistent Company"
            };

            Func<Task> act = async () => { await _sut.UpdateAsync(companyToUpdate); };
            act.Should().Throw<NullReferenceException>();
        }

        [Test]
        public async Task When_Company_Gets_Updated_Should_Call_SaveChanges()
        {
            var companyToUpdate = new CompanyModel
            {
                Id = 3,
                Name = "Third Company Updated Name",
                Domains = new List<DomainModel>()
            };

            await _sut.UpdateAsync(companyToUpdate);

            _dbContextMock.Verify(x => x.SaveChangesAsync());
        }

        [Test]
        public async Task When_Company_Gets_Updated_Should_Update_Changed_Properties()
        {
            var companyToUpdate = new CompanyModel
            {
                Id = 3,
                Name = "Third Company Updated Name",
                Domains = new List<DomainModel>()
            };

            await _sut.UpdateAsync(companyToUpdate);

            var updatedCompany = _companiesDbSet.FirstOrDefault(c => c.Id == 3);
            updatedCompany?.Name.Should().Be("Third Company Updated Name");
        }

        [Test]
        public async Task When_Company_Gets_Updated_Should_Return_Updated_Model()
        {
            var companyToUpdate = new CompanyModel
            {
                Id = 2,
                Name = "Second Company Updated Name",
                Domains = new List<DomainModel>()
            };

            var returnedModel = await _sut.UpdateAsync(companyToUpdate);

            returnedModel.Should().NotBeNull();
            returnedModel.Name.Should().Be("Second Company Updated Name");
        }

        [Test]
        public async Task When_New_Domains_Have_Been_Provided_Should_Increase_Domains_Count()
        {
            var companyToUpdate = new CompanyModel
            {
                Id = 1,
                Name = "First Company",
                Domains = new List<DomainModel>
                {
                    new DomainModel { Id = 7, Name = "Third Domain of First Company" },
                    new DomainModel { Id = 7, Name = "Forth Domain of First Company"}
                }
            };

            await _sut.UpdateAsync(companyToUpdate);

            var numOfDomains = _domainsDbSet.Count();
            numOfDomains.Should().Be(2);
        }
    }
}
