using CloudPlus.Database;
using CloudPlus.Entities.Office365;
using CloudPlus.Models.Office365.UserGroup;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Services.Database.Office365.UserGroup
{
    public class Office365DbUserGroupService : IOffice365DbUserGroupService
    {
        private readonly CldpDbContext _dbContext;

        public Office365DbUserGroupService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Office365SecurityUserGroupMemberModel
        public async Task<List<Office365SecurityGroupMemberModel>> GetOffice365SecurityGroupUserAsync(string userPrincipalName)
        {
            var office365SecurityGroupUser =await _dbContext.Office365SecurityGroupMembers.Where(x=>x.UserPrincipalName==userPrincipalName)
                .Select(x=>new Office365SecurityGroupMemberModel { SecurityGroupName=x.SecurityGroupName, UserPrincipalName=x.UserPrincipalName }).ToListAsync();

            if (office365SecurityGroupUser == null)
                return null;

            return office365SecurityGroupUser;
        }

        public async Task<List<Office365DistributionGroupMemberModel>> GetOffice365DistributionGroupUserAsync(string userPrincipalName)
        {
            var office365DistriputionGroupUser = await _dbContext.Office365AndDistributionGroupMembers.Where(x => x.UserPrincipalName == userPrincipalName && x.GroupType== Office365GroupType.Distribution)
                .Select(x => new Office365DistributionGroupMemberModel {  DistributionGroupName = x.GroupName, UserPrincipalName = x.UserPrincipalName }).ToListAsync();

            if (office365DistriputionGroupUser == null)
                return null;

            return office365DistriputionGroupUser;
        }

        public async Task<List<Office365GroupMemberModel>> GetOffice365GroupUserAsync(string userPrincipalName)
        {
            var office365GroupUser = await _dbContext.Office365AndDistributionGroupMembers.Where(x => x.UserPrincipalName == userPrincipalName && x.GroupType == Office365GroupType.Office365)
                .Select(x => new Office365GroupMemberModel {  Office365GroupName = x.GroupName, UserPrincipalName = x.UserPrincipalName }).ToListAsync();

            if (office365GroupUser == null)
                return null;

            return office365GroupUser;
        }


        public async Task CreateOffice365SecurityGroupUserAsync(Office365SecurityGroupMemberModel model)
        {
            _dbContext.Office365SecurityGroupMembers.Add(new Office365SecurityGroupMember
            {
                UserPrincipalName = model.UserPrincipalName,
                SecurityGroupName=model.SecurityGroupName
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateOffice365DistributionGroupUserAsync(Office365DistributionGroupMemberModel model)
        {
            _dbContext.Office365AndDistributionGroupMembers.Add(new Office365AndDistributionGroupMember
            {
                UserPrincipalName = model.UserPrincipalName,
                 GroupName = model.DistributionGroupName,
                  GroupType= Office365GroupType.Distribution
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateOffice365GroupUserAsync(Office365GroupMemberModel model)
        {
            _dbContext.Office365AndDistributionGroupMembers.Add(new Office365AndDistributionGroupMember
            {
                UserPrincipalName = model.UserPrincipalName,
                GroupName = model.Office365GroupName,
                GroupType = Office365GroupType.Office365
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateOffice365SecurityGroupAsync(Office365SecurityGroupModel model)
        {
            _dbContext.Office365SecurityGroups.Add(new Office365SecurityGroup
            {
                UserPrincipalName = model.UserPrincipalName,
                 Office365GroupName = model.SecurityGroupName
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateOffice365DistributionGroupAsync(Office365DistributionGroupModel model)
        {
            _dbContext.Office365AndDistributionGroups.Add(new Office365AndDistributionGroup
            {
                UserPrincipalName = model.UserPrincipalName,
                 Office365GroupName = model.DistributionGroupName,
                GroupType = Office365GroupType.Distribution
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateOffice365GroupAsync(Office365GroupModel model)
        {
            _dbContext.Office365AndDistributionGroups.Add(new Office365AndDistributionGroup
            {
                UserPrincipalName = model.UserPrincipalName,
                Office365GroupName = model.Office365GroupName,
                GroupType = Office365GroupType.Office365
            });
            await _dbContext.SaveChangesAsync();
        }


        public async Task RemoveOffice365SecurityGroupUserAsync(Office365SecurityGroupMemberModel model)
        {
            _dbContext.Office365SecurityGroupMembers.Add(new Office365SecurityGroupMember
            {
                UserPrincipalName = model.UserPrincipalName,
                SecurityGroupName = model.SecurityGroupName
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveOffice365DistributionGroupUserAsync(Office365DistributionGroupMemberModel model)
        {
            _dbContext.Office365AndDistributionGroupMembers.Add(new Office365AndDistributionGroupMember
            {
                UserPrincipalName = model.UserPrincipalName,
                GroupName = model.DistributionGroupName,
                GroupType = Office365GroupType.Distribution
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveOffice365GroupUserAsync(Office365GroupMemberModel model)
        {
            _dbContext.Office365AndDistributionGroupMembers.Add(new Office365AndDistributionGroupMember
            {
                UserPrincipalName = model.UserPrincipalName,
                GroupName = model.Office365GroupName,
                GroupType = Office365GroupType.Office365
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveOffice365SecurityGroupAsync(Office365SecurityGroupModel model)
        {
            _dbContext.Office365SecurityGroups.Add(new Office365SecurityGroup
            {
                UserPrincipalName = model.UserPrincipalName,
                Office365GroupName = model.SecurityGroupName
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveOffice365DistributionGroupAsync(Office365DistributionGroupModel model)
        {
            _dbContext.Office365AndDistributionGroups.Add(new Office365AndDistributionGroup
            {
                UserPrincipalName = model.UserPrincipalName,
                Office365GroupName = model.DistributionGroupName,
                GroupType = Office365GroupType.Distribution
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveOffice365GroupAsync(Office365GroupModel model)
        {
            _dbContext.Office365AndDistributionGroups.Add(new Office365AndDistributionGroup
            {
                UserPrincipalName = model.UserPrincipalName,
                Office365GroupName = model.Office365GroupName,
                GroupType = Office365GroupType.Office365
            });
            await _dbContext.SaveChangesAsync();
        }


    }
}
