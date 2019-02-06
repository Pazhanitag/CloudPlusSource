using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Office365;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Identity;
using CloudPlus.Models.Office365.License;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.User;
using CloudPlus.Models.Provisions;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Services.Database.Office365.User
{
    public class Office365DbUserService : IOffice365DbUserService
    {
        private readonly CldpDbContext _dbContext;
        private readonly IUserService _userService;

        public Office365DbUserService(CldpDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<Office365UserModel> GetOffice365DatabaseUserAsync(string userPrincipalName)
        {
            var office365User = await GetOffice365DatabaseUserEntityAsync(userPrincipalName);

            if (office365User == null)
                return null;

            return new Office365UserModel
            {
                Id = office365User.Id,
                Office365UserId = office365User.Office365UserId,
                CloudPlusUserId = office365User.CloudPlusUserId,
                UserPrincipalName = office365User.UserPrincipalName,
                CustomerId = office365User.CustomerId,
                Office365UserState = office365User.Office365UserState,
                Office365SoftDeletionTime = office365User.Office365SoftDeletionTime,
                Licenses = office365User.Licenses.Select(l => new Office365LicenseModel
                {
                    Office365Offer = new Office365OfferModel
                    {
                        Id = l.Office365Offer.Id,
                        CloudPlusProductIdentifier = l.Office365Offer.CloudPlusProductIdentifier,
                        Sku = l.Office365Offer.Office365ProductSku,
                        Office365Id = l.Office365Offer.Office365OfferId
                    }
                })
            };
        }

        public async Task<Office365UserModel> GetOffice365DatabaseUserWithLicensesAndOfferAsync(string userPrincipalName)
        {
            var office365User = await GetOffice365DatabaseUserEntityWithLicensesAndOfferAsync(userPrincipalName);

            return new Office365UserModel
            {
                Id = office365User.Id,
                Office365UserId = office365User.Office365UserId,
                CloudPlusUserId = office365User.CloudPlusUserId,
                UserPrincipalName = office365User.UserPrincipalName,
                CustomerId = office365User.CustomerId,
                Office365UserState = office365User.Office365UserState,
                Office365SoftDeletionTime = office365User.Office365SoftDeletionTime,
                Licenses = office365User.Licenses.Select(item => new Office365LicenseModel
                {
                    Id = item.Id,
                    Office365Offer = new Office365OfferModel
                    {
                        Id = item.Office365Offer.Id,
                        Office365Id = item.Office365Offer.Office365OfferId,
                        OfferName = item.Office365Offer.Office365OfferName,
                        Sku = item.Office365Offer.Office365ProductSku,
                        CloudPlusProductIdentifier = item.Office365Offer.CloudPlusProductIdentifier
                    }
                })
            };
        }

        public async Task<List<Office365UserModel>> GetAllCustomerUsersWithLicensesAndOfferAsync(int customerId)
        {
            var office365Users = await _dbContext.Office365Users
                .Include(i => i.Licenses.Select(o => o.Office365Offer))
                .Where(u => u.CustomerId == customerId).ToListAsync();

            return office365Users.Select(u => new Office365UserModel
            {
                Id = u.Id,
                Office365UserId = u.Office365UserId,
                CloudPlusUserId = u.CloudPlusUserId,
                UserPrincipalName = u.UserPrincipalName,
                CustomerId = u.CustomerId,
                Office365UserState = u.Office365UserState,
                Office365SoftDeletionTime = u.Office365SoftDeletionTime,
                Licenses = u.Licenses.Select(item => new Office365LicenseModel
                {
                    Id = item.Id,
                    Office365Offer = new Office365OfferModel
                    {
                        Id = item.Office365Offer.Id,
                        Office365Id = item.Office365Offer.Office365OfferId,
                        OfferName = item.Office365Offer.Office365OfferName,
                        Sku = item.Office365Offer.Office365ProductSku,
                        CloudPlusProductIdentifier = item.Office365Offer.CloudPlusProductIdentifier
                    }
                })
            }).ToList();
        }

        public async Task<Office365UserState?> GetOffice365UserState(string userPrincipalName)
        {
            var user = await _dbContext.Office365Users.FirstOrDefaultAsync(u => u.UserPrincipalName == userPrincipalName);

            return user?.Office365UserState;
        }

        public async Task<Office365UserModel> CreateOffice365DatabaseUserAsync(Office365UserModel model)
        {
            var user = _userService.GetUser(model.UserPrincipalName);

            if (user == null)
                throw new ArgumentException($"No database user with User Principal Name: {model.UserPrincipalName}");

            var customer = _dbContext.Office365Customers.FirstOrDefault(c => c.CompanyId == user.CompanyId);

            if (customer == null)
                throw new ArgumentException($"No Office 365 Database Customer with Company Id: {user.CompanyId}");

            var createdUser = _dbContext.Office365Users.Add(new Office365User
            {
                Customer = customer,
                CloudPlusUserId = user.Id,
                Office365UserId = model.Office365UserId,
                UserPrincipalName = model.UserPrincipalName,
            });

            await _dbContext.SaveChangesAsync();

            model.Id = createdUser.Id;

            return model;
        }

        public async Task<Office365UserModel> ActivateOffice365DatabaseUserAsync(string userPrincipalName)
        {
            var office365User = await GetOffice365DatabaseUserEntityWithLicensesAndOfferAsync(userPrincipalName);

            office365User.Office365UserState = Office365UserState.Active;

            await _dbContext.SaveChangesAsync();

            return new Office365UserModel
            {
                Id = office365User.Id,
                Office365UserId = office365User.Office365UserId,
                CloudPlusUserId = office365User.CloudPlusUserId,
                UserPrincipalName = office365User.UserPrincipalName,
                CustomerId = office365User.CustomerId,
                Office365UserState = office365User.Office365UserState,
                Office365SoftDeletionTime = office365User.Office365SoftDeletionTime,
                Licenses = office365User.Licenses.Select(item => new Office365LicenseModel
                {
                    Id = item.Id,
                    Office365Offer = new Office365OfferModel
                    {
                        Id = item.Office365Offer.Id,
                        Office365Id = item.Office365Offer.Office365OfferId,
                        OfferName = item.Office365Offer.Office365OfferName,
                        Sku = item.Office365Offer.Office365ProductSku,
                        CloudPlusProductIdentifier = item.Office365Offer.CloudPlusProductIdentifier
                    }
                })
            };
        }

        public async Task DeleteOffice365DatabaseUserAsync(string userPrincipalName)
        {
            var office365User =
                await _dbContext.Office365Users.Include(u => u.Licenses).FirstOrDefaultAsync(u => u.UserPrincipalName == userPrincipalName);

            if (office365User == null)
                throw new Exception($"Could not find user with upn {userPrincipalName}");

            _dbContext.Office365Licenses.RemoveRange(office365User.Licenses);
            _dbContext.Office365Users.Remove(office365User);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SoftDeleteOffice365DatabaseUserAsync(string userPrincipalName)
        {
            var office365User = await _dbContext.Office365Users
                .FirstOrDefaultAsync(u => u.UserPrincipalName == userPrincipalName);

            office365User.Office365UserState = Office365UserState.Inactive;
            office365User.Office365SoftDeletionTime = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        private async Task<Office365User> GetOffice365DatabaseUserEntityAsync(string userPrincipalName)
        {
            var office365User = await _dbContext.Office365Users.AsNoTracking()
                .Include(u => u.Licenses.Select(l => l.Office365Offer))
                .FirstOrDefaultAsync(u => u.UserPrincipalName == userPrincipalName);
            
            return office365User;
        }

        private async Task<Office365User> GetOffice365DatabaseUserEntityWithLicensesAndOfferAsync(string userPrincipalName)
        {
            var office365User = await _dbContext.Office365Users
                .Include(i => i.Licenses.Select(o => o.Office365Offer))
                .FirstOrDefaultAsync(u => u.UserPrincipalName == userPrincipalName);

            if (office365User == null)
                throw new ArgumentException($"No database Office 365 user with User Principal Name: {userPrincipalName}");

            return office365User;
        }

        public async Task<IEnumerable<UserModel>> GetUsersByDomainAsync(string domain, string searchTerm, int companyId)
        {
            var users = _userService.GetUsersByDomain(domain, companyId).ToList();
            var office365Customer = await _dbContext.Office365Customers.FirstOrDefaultAsync(u => u.CompanyId == companyId);

            var office365Users = await _dbContext.Office365Users
                .Include(i => i.Licenses.Where(o=>o.IsDeleted == false).Select(o => o.Office365Offer))
                .Where(u => u.CustomerId == office365Customer.Id).ToListAsync();

            var dbProductItems = _dbContext.ProductItems.ToList();

            foreach (var item in office365Users)
            {
                var user = users.FirstOrDefault(u => u.Email == item.UserPrincipalName);
                if (user == null) continue;

                var assignedLicense = item.Licenses.ToList().Select(x => new AssignedServicesModel() { cloudPlusProductIdentifier = x.Office365Offer.CloudPlusProductIdentifier, offerName = x.Office365Offer.Office365OfferName }).ToList();
                //item.Licenses.ToList();
                if(assignedLicense.Count() > 0)
                {
                    user.AssignedLicenses = assignedLicense;
                    user.AssignedLicense = string.Join(", ", assignedLicense.Select(x => x.offerName).ToArray());
                }
                //user.AssignedLicense = dbProductItems.FirstOrDefault(i =>
                //    i.Identifier == assignedLicense.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier)?.Name;

                if (item.Office365UserState == Office365UserState.Inactive)
                    users.Remove(user);
                else
                    user.IsProvisioned = true;

            }

            searchTerm = searchTerm.ToLower();

            return users.Where(user => 
            user.DisplayName.ToLower().Contains(searchTerm)
            || user.FirstName.ToLower().Contains(searchTerm)
            || user.LastName.ToLower().Contains(searchTerm)
            || user.Email.ToLower().Contains(searchTerm)
            || user.AssignedLicense.ToLower().Contains(searchTerm));
        }
    }
}
