using CloudPlus.Models.Office365.UserGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Services.Database.Office365.UserGroup
{
    public interface IOffice365DbUserGroupService
    {
        Task<List<Office365SecurityGroupMemberModel>> GetOffice365SecurityGroupUserAsync(string userPrincipalName);
        Task<List<Office365DistributionGroupMemberModel>> GetOffice365DistributionGroupUserAsync(string userPrincipalName);
        Task<List<Office365GroupMemberModel>> GetOffice365GroupUserAsync(string userPrincipalName);

        Task CreateOffice365SecurityGroupUserAsync(Office365SecurityGroupMemberModel model);
        Task CreateOffice365DistributionGroupUserAsync(Office365DistributionGroupMemberModel model);
        Task CreateOffice365GroupUserAsync(Office365GroupMemberModel model);

        Task CreateOffice365SecurityGroupAsync(Office365SecurityGroupModel model);
        Task CreateOffice365DistributionGroupAsync(Office365DistributionGroupModel model);
        Task CreateOffice365GroupAsync(Office365GroupModel model);

        Task RemoveOffice365SecurityGroupUserAsync(Office365SecurityGroupMemberModel model);
        Task RemoveOffice365DistributionGroupUserAsync(Office365DistributionGroupMemberModel model);
        Task RemoveOffice365GroupUserAsync(Office365GroupMemberModel model);

        Task RemoveOffice365SecurityGroupAsync(Office365SecurityGroupModel model);
        Task RemoveOffice365DistributionGroupAsync(Office365DistributionGroupModel model);
        Task RemoveOffice365GroupAsync(Office365GroupModel model);
    }
}
