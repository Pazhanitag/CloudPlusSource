using System.Collections.Generic;
using CloudPlus.Database;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Database.Tests.Domain.DomainService
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class GetCompanyDomains_Specs
    {
        // System Under Test
        private Database.Domain.DomainService _sut;

        [SetUp]
        public void SetUp()
        {
            var domains = new List<Entities.Domain>
            {
                new Entities.Domain
                {
                    Id = 1,
                    Name = "First Domain",
                    CompanyId = 1
                },
                new Entities.Domain
                {
                    Id = 2,
                    Name = "Second Domain",
                    CompanyId = 1
                },
                new Entities.Domain
                {
                    Id = 3,
                    Name = "Third Domain",
                    CompanyId = 2
                },
            };

            var domainsDbSet = MockSetGenerator.CreateMockSet(domains);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Domains).Returns(domainsDbSet);

            _sut = new Database.Domain.DomainService(dbContextMock.Object);
        }

        [Test]
        public void When_No_Domains_With_Passed_CompanyId_Exist_Should_Return_Empty_Enumerable()
        {
            var domains = _sut.GetCompanyDomains(3);
            domains.Should().BeEmpty();
        }

        [Test]
        public void When_Domains_With_Passed_CompanyId_Exist_Should_Return_Domains()
        {
            var domains = _sut.GetCompanyDomains(1);
            domains.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.NotContain(domain => domain.Name == "Third Domain");
        }
    }
}
