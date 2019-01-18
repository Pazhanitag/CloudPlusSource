using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database.Authentication;
using CloudPlus.Entities.Identity;
using CloudPlus.Enums.User;
using CloudPlus.Models.Identity;
using CloudPlus.Resources;
using CloudPlus.Models.Company;

namespace CloudPlus.Services.Identity.User
{
    public class UserService : IUserService
    {
        private readonly CloudPlusAuthDbContext _dbContext;
        private readonly IConfigurationManager _configurationManager;

        public UserService(CloudPlusAuthDbContext dbContext, IConfigurationManager configurationManager)
        {
            _dbContext = dbContext;
            _configurationManager = configurationManager;
        }

        public IEnumerable<UserModel> SearchUsers(string searchTerm, int companyId)
        {
            searchTerm = searchTerm.ToLower();
            return _dbContext.Users.Where(user => (
                user.FirstName.ToLower().Contains(searchTerm)
                || user.LastName.ToLower().Contains(searchTerm)
                || user.Email.ToLower().Contains(searchTerm)
                || user.UserStatus.ToString().Contains(searchTerm)
                || user.Roles.Where(r => r.Role.Name.ToLower().Contains(searchTerm)).Select(r => r.Role.Name).Any()) && user.CompanyId == companyId)
                .Select(user => new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    UserStatus = user.UserStatus,
                    CompanyId = user.CompanyId,
                    UserName = user.UserName,
                    City = user.City,
                    Country = user.Country,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    StreetAddress = user.StreetAddress,
                    JobTitle = user.JobTitle,
                    AlternativeEmail = user.AlternativeEmail,
                    CompanyName = user.CompanyName,
                    Roles = user.Roles.Select(x => new RoleModel
                    {
                        Id = x.Role.Id,
                        Name = x.Role.Name,
                        FriendlyName = x.Role.FriendlyName
                    })
                });
        }

        public async Task<UserModel> GetUserAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var user = await _dbContext.Users.Include(u => u.Roles.Select(r => r.Role))
                             .FirstOrDefaultAsync(u => u.Email.Equals(email) || u.AlternativeEmail.Equals(email));

            if (user == null)
                return null;

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                CompanyId = user.CompanyId,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                AlternativeEmail = user.AlternativeEmail,
                CompanyName = user.CompanyName,
                Roles = user.Roles.Select(x => new RoleModel
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                })
            };
        }

        public UserModel GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var user = _dbContext.Users.Include(u => u.Roles.Select(r => r.Role)).FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                CompanyId = user.CompanyId,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                AlternativeEmail = user.AlternativeEmail,
                CompanyName = user.CompanyName,
                Roles = user.Roles.Select(x => new RoleModel
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                })
            };
        }

        public async Task<UserModel> GetUserAsync(int id)
        {
            var user = await _dbContext.Users.Include(u => u.Roles.Select(r => r.Role)).FirstOrDefaultAsync(u => u.Id == id);
            return user != null ? ToUserModel(user) : null;
        }
        public async Task<UserModel> GetUserAsync(int id, int companyId)
        {
            var user = await _dbContext.Users.Include(u => u.Roles.Select(r => r.Role)).FirstOrDefaultAsync(u => u.Id == id && u.CompanyId == companyId);
            return user != null ? ToUserModel(user) : null;
        }

        public UserModel GetUser(int id)
        {
            var user = _dbContext.Users.Include(u => u.Roles.Select(r => r.Role)).FirstOrDefault(u => u.Id == id);
            return user != null ? ToUserModel(user) : null;
        }

        public async Task<UserModel> CreateAsync(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var user = MapToUserEntity(model);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Users.Add(user);

                    var roles = _dbContext.Roles.AsEnumerable().Where(r => model.Roles.Select(mr => mr.Id).Contains(r.Id)).ToList();

                    if (roles.Any())
                    {
                        var userRoles = roles.Select(role => new UserRole
                        {
                            RoleId = role.Id,
                            UserId = user.Id
                        });
                        _dbContext.UserRoles.AddRange(userRoles);
                    }

                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return ToUserModel(user);
                }
                catch (Exception)
                {
                    transaction?.Rollback();

                    throw;
                }
            }
        }

        public UserModel Create(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var user = MapToUserEntity(model);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Users.Add(user);

                    foreach (var modelRole in model.Roles)
                    {
                        _dbContext.UserRoles.Add(new UserRole
                        {
                            RoleId = modelRole.Id,
                            UserId = user.Id
                        });
                    }

                    _dbContext.SaveChanges();

                    transaction.Commit();

                    return ToUserModel(user);
                }
                catch (Exception)
                {
                    transaction?.Rollback();

                    throw;
                }
            }
        }

        public async Task<UserModel> UpdateAsync(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                    if (user == null)
                        throw new NullReferenceException(nameof(user));

                    ModelToEntity(model, user);

                    if(model.Roles != null)
                    {
                        // Comment on my own code by Kljuco 10-08-17: OMG!!!! It has to be more simple than this!
                        var userRoles = _dbContext.UserRoles.AsEnumerable().Where(u => u.UserId == user.Id).ToList();
                        if (userRoles.Select(ur => ur.RoleId).Intersect(model.Roles.Select(mr => mr.Id)).Count() != model.Roles.Count())
                        {
                            var rolesToBeAdded = model.Roles.Select(mr => mr.Id).Except(userRoles.Select(ur => ur.RoleId));
                            var rolesToBeRemoved = userRoles.Select(ur => ur.RoleId).Except(model.Roles.Select(mr => mr.Id));

                            foreach (var roleId in rolesToBeAdded)
                            {
                                _dbContext.UserRoles.Add(new UserRole
                                {
                                    RoleId = roleId,
                                    UserId = user.Id
                                });
                            }
                            foreach (var roleId in rolesToBeRemoved)
                            {
                                _dbContext.UserRoles.Remove(userRoles.FirstOrDefault(r => r.RoleId == roleId) ?? throw new InvalidOperationException());
                            }
                        }
                    }
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return model;
                }
                catch (Exception)
                {
                    transaction?.Rollback();

                    throw;
                }
            }
        }

        public async Task<bool> IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) || u.AlternativeEmail.Equals(email));

            return user != null;
        }

        public async Task<bool> IsDisplayNameAvailable(string displayName, int companyId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.DisplayName == displayName && u.CompanyId == companyId);

            return user == null;
        }
        public IEnumerable<UserModel> GetUsers(int companyId)
        {
            return _dbContext.Users.Where(u => u.CompanyId == companyId).Select(user => new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                CompanyId = user.CompanyId,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                AlternativeEmail = user.AlternativeEmail,
                CompanyName = user.CompanyName,
                Roles = user.Roles.Select(x => new RoleModel
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name,
                    FriendlyName = x.Role.FriendlyName
                })
            });
        }

        public UserModel Update(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Id == model.Id);
                    if (user == null)
                        throw new NullReferenceException(nameof(user));

                    ModelToEntity(model, user);

                    // Comment on my own code by Kljuco 10-08-17: OMG!!!! It has to be more simple than this!
                    var userRoles = _dbContext.UserRoles.Where(u => u.UserId == user.Id).ToList();
                    if (userRoles.Select(ur => ur.RoleId).Intersect(model.Roles.Select(mr => mr.Id)).Count() != model.Roles.Count())
                    {
                        var rolesToBeAdded = model.Roles.Select(mr => mr.Id).Except(userRoles.Select(ur => ur.RoleId));
                        var rolesToBeRemoved = userRoles.Select(ur => ur.RoleId).Except(model.Roles.Select(mr => mr.Id));

                        foreach (var roleId in rolesToBeAdded)
                        {
                            _dbContext.UserRoles.Add(new UserRole
                            {
                                RoleId = roleId,
                                UserId = user.Id
                            });
                        }
                        foreach (var roleId in rolesToBeRemoved)
                        {
                            _dbContext.UserRoles.Remove(userRoles.FirstOrDefault(r => r.Id == roleId) ?? throw new InvalidOperationException());
                        }
                    }
                    _dbContext.SaveChanges();

                    transaction.Commit();

                    return model;
                }
                catch (Exception)
                {
                    transaction?.Rollback();

                    throw;
                }
            }
        }

        public bool ResetSecurityStamp(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(username));

            user.SecurityStamp = Guid.NewGuid().ToString();

            _dbContext.SaveChanges();

            return true;
        }

        public void Delete(int id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

                    if (user == null)
                        throw new NullReferenceException(nameof(user));

                    var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == id);

                    _dbContext.Users.Remove(user);
                    _dbContext.UserRoles.RemoveRange(userRoles);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction?.Rollback();

                    throw;
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                    if (user == null)
                        throw new NullReferenceException(nameof(user));

                    var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == id);

                    _dbContext.Users.Remove(user);
                    _dbContext.UserRoles.RemoveRange(userRoles);

                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction?.Rollback();

                    throw;
                }
            }
        }


        public async Task GenerateImpersonateToken(int parentUserId, int impersonateUserId)
        {
            var parentUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == parentUserId);
            var impersonateUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == impersonateUserId);

            if (parentUser == null || impersonateUser == null)
            {
                throw new Exception("Invalid user");
            }

            _dbContext.Impersonates.Add(new Impersonate
            {
                ParentUser = parentUser,
                ImpersonateUser = impersonateUser
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsImpersonateTokenValid(int parentUserId, int impersonateUserId)
        {
            var impersonateToken = await _dbContext.Impersonates.Include(incl => incl.ParentUser).Include(incl => incl.ImpersonateUser)
                .FirstOrDefaultAsync(impersonate => impersonate.ParentUser.Id == parentUserId && impersonate.ImpersonateUser.Id == impersonateUserId);

            if (impersonateToken == null)
            {
                return false;
            }

            if (DateTime.Compare(impersonateToken.CreatedAt.AddSeconds(30), DateTime.UtcNow.AddSeconds(30)) > 0)
                return false;

            _dbContext.Impersonates.Remove(impersonateToken);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<UserModel> GetUserByAlternativeEmailAsync(string alternativeEmail)
        {
            if (string.IsNullOrWhiteSpace(alternativeEmail))
                throw new ArgumentNullException(nameof(alternativeEmail));

            var user = await _dbContext.Users.Include(u => u.Roles.Select(r => r.Role))
                .FirstOrDefaultAsync(u => u.AlternativeEmail.Equals(alternativeEmail));

            if (user == null)
                return null;

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                CompanyId = user.CompanyId,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                AlternativeEmail = user.AlternativeEmail,
                CompanyName = user.CompanyName,
                Roles = user.Roles.Select(x => new RoleModel
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                })
            };
        }

        public IEnumerable<UserModel> GetUsersByDomain(string domain, int comapanyId)
        {
            var fullDomain = string.Concat("@", domain);
            return _dbContext.Users.Where(u => 
            u.Email.Contains(fullDomain) && 
            u.CompanyId == comapanyId && 
            u.UserStatus == UserStatus.Active).Select(user => new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        public CompanyCountModel GetUserCountForCutomers (CompanyCountModel root)
        {

            root.NumberOfUsers = _dbContext.Users.Count(user => user.CompanyId == root.Id);

            return root;
        }
        private void ModelToEntity(UserModel model, Entities.Identity.User user)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DisplayName = model.DisplayName;
            user.CompanyName = model.CompanyName;
            user.JobTitle = model.JobTitle;
            user.AlternativeEmail = model.AlternativeEmail;
            user.Country = model.Country;
            user.State = model.State;
            user.ZipCode = model.ZipCode;
            user.StreetAddress = model.StreetAddress;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.UserStatus = model.UserStatus;
            if (!string.IsNullOrWhiteSpace(model.ProfilePicture))
                user.ProfilePicture = model.ProfilePicture;

            user.SecurityStamp = Guid.NewGuid().ToString();
        }

        private Entities.Identity.User MapToUserEntity(UserModel model)
        {
            return new Entities.Identity.User
            {
                Email = model.Email,
                DisplayName = model.DisplayName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ProfilePicture = model.ProfilePicture,
                CompanyId = model.CompanyId,
                UserName = model.UserName,
                City = model.City,
                Country = model.Country,
                State = model.State,
                ZipCode = model.ZipCode,
                StreetAddress = model.StreetAddress,
                JobTitle = model.JobTitle,
                AlternativeEmail = model.AlternativeEmail,
                CompanyName = model.CompanyName,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserStatus = model.UserStatus,
                LockoutEnabled = Convert.ToBoolean(_configurationManager.GetByKey("UserLockoutEnabledByDefault"))
            };
        }

        private UserModel ToUserModel(Entities.Identity.User user)
        {
            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                CompanyId = user.CompanyId,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                AlternativeEmail = user.AlternativeEmail,
                CompanyName = user.CompanyName,
                Roles = user.Roles.Select(x => new RoleModel
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name,
                    FriendlyName = x.Role.FriendlyName
                })
            };
        }
    }
}

