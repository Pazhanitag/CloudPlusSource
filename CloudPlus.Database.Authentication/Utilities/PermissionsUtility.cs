using System.Linq;
using CloudPlus.Entities.Identity;

namespace CloudPlus.Database.Authentication.Utilities
{
    public class PermissionsUtility
    {
        private readonly CloudPlusAuthDbContext _context;

        public PermissionsUtility(CloudPlusAuthDbContext context)
        {
            _context = context;
        }

        public Permission ViewUsersPermission => _context.Permissions.FirstOrDefault(w => w.Name == "ViewUsers");
        public Permission EditUsersPermission => _context.Permissions.FirstOrDefault(w => w.Name == "EditUsers");
        public Permission AddUsersPermission => _context.Permissions.FirstOrDefault(w => w.Name == "AddUsers");
        public Permission DeleteUsersPermission => _context.Permissions.FirstOrDefault(w => w.Name == "DeleteUsers");
        public Permission ViewAccountsPermission => _context.Permissions.FirstOrDefault(w => w.Name == "ViewAccounts");
        public Permission EditAccountsPermission => _context.Permissions.FirstOrDefault(w => w.Name == "EditAccounts");
        public Permission AddAccountsPermission => _context.Permissions.FirstOrDefault(w => w.Name == "AddAccounts");
        public Permission DeleteAccountsPermission => _context.Permissions.FirstOrDefault(w => w.Name == "DeleteAccounts");
        public Permission ViewPriceCatalogPermission => _context.Permissions.FirstOrDefault(w => w.Name == "ViewPriceCatalog");
        public Permission SetMsrpFixedPermission => _context.Permissions.FirstOrDefault(w => w.Name == "SetMsrpFixed");
        public Permission ViewProductCatalogPermission => _context.Permissions.FirstOrDefault(w => w.Name == "ViewProductCatalog");
        public Permission EditMyCompany => _context.Permissions.FirstOrDefault(w => w.Name == "EditMyCompany");
        public Permission ViewExternalSignupLink => _context.Permissions.FirstOrDefault(w => w.Name == "ViewExternalSignupLink");
	    public Permission ViewMyCompany => _context.Permissions.FirstOrDefault(w => w.Name == "ViewMyCompany");
	    public Permission EditMyProfile => _context.Permissions.FirstOrDefault(w => w.Name == "EditMyProfile");
	}
}
