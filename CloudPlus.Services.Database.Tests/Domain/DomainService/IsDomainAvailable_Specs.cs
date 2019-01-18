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
    public class IsDomainAvailable_Specs
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
                },
                new Entities.Domain
                {
                    Id = 2,
                    Name = "Second Domain",
                },
                new Entities.Domain
                {
                    Id = 3,
                    Name = "Third Domain",
                },
            };

            var domainsDbSet = MockSetGenerator.CreateMockSet(domains);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(c => c.Domains).Returns(domainsDbSet);

            _sut = new Database.Domain.DomainService(dbContextMock.Object);
        }

        [Test]
        public void When_Domain_Exists_Should_Return_False()
        {
            var domain = _sut.IsDomainAvailable("First Domain");
            domain.Should().BeFalse();
        }

        [Test]
        public void When_Domain_Does_Not_Exist_Should_Return_True()
        {
            var domain = _sut.IsDomainAvailable("Nonexistent Domain");
            domain.Should().BeTrue();
        }
    }
}
