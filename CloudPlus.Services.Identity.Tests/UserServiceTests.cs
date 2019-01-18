using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CloudPlus.Database.Authentication;
using CloudPlus.Entities.Identity;
using CloudPlus.Resources;
using CloudPlus.Services.Identity.User;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudPlus.Services.Identity.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<CloudPlusAuthDbContext> _dbContext;
        private IQueryable<User> _usersMock;
        private Mock<DbSet<User>> _userMockSet;

        [SetUp]
        public void Init()
        {
            _configurationManager = new Mock<IConfigurationManager>();
            _dbContext = new Mock<CloudPlusAuthDbContext>();
            _usersMock = new List<User>
            {
                new User
                {
                    UserName = "dalilaav@maestralsolutions.com",
                    Email = "dalilaav@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Dalila",
                    LastName = "Avdukic",
                    IsActive = true,
                    AlternativeEmail = "dalila@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "amerra@maestralsolutions.com",
                    Email = "amerra@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Amer",
                    LastName = "Ratkovic",
                    IsActive = true,
                    AlternativeEmail = "amer@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "farisfe@maestralsolutions.com",
                    Email = "farisfe@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Faris",
                    LastName = "Fetahagic",
                    IsActive = true,
                    AlternativeEmail = "amer@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "mersihase@maestralsolutions.com",
                    Email = "mersihase@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Mersiha",
                    LastName = "Seljpic",
                    IsActive = true,
                    AlternativeEmail = "mersiha@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "uzeir@maestralsolutions.com",
                    Email = "uzeir@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Uzeir",
                    LastName = "Basic",
                    IsActive = true,
                    AlternativeEmail = "mersiha@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "emir@maestralsolutions.com",
                    Email = "emir@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Emir",
                    LastName = "Kljucanin",
                    IsActive = true,
                    AlternativeEmail = "kljuco@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "seidso@maestralsolutions.com",
                    Email = "seidso@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Seid",
                    LastName = "Solak",
                    IsActive = true,
                    AlternativeEmail = "sejo@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                },
                new User
                {
                    UserName = "adnanmu@maestralsolutions.com",
                    Email = "adnanmu@maestralsolutions.com",
                    EmailConfirmed = true,
                    FirstName = "Adnan",
                    LastName = "Mulalic",
                    IsActive = true,
                    AlternativeEmail = "adnan@test.test",
                    CompanyId = 3,
                    LockoutEnabled = true
                }
            }.AsQueryable();
            _userMockSet = MockSetGenerator.CreateMockSet(_usersMock);
            _dbContext.SetupGet(c => c.Users).Returns(_userMockSet.Object);

        }

        [Test]
        public void When_getting_users_without_search_term_Should_return_all_users()
        {
            var userService = new UserService(_dbContext.Object, _configurationManager.Object);

            var users = userService.SearchUsers(string.Empty, 3);

            users.Count().ShouldBeEquivalentTo(_usersMock.Count());
        }
    }
}
