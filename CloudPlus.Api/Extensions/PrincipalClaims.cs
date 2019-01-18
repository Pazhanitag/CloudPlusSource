using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace CloudPlus.Api.Extensions
{
    public static class PrincipalClaims
    {
        public static int CompanyId(this IPrincipal principal)
        {
            var claimPrincipal = principal as ClaimsPrincipal;

            var companyIdClaim = claimPrincipal?.Claims.FirstOrDefault(c => c.Type == "company_id");

            if (companyIdClaim == null)
            {
                throw new Exception("Invalid company");
            }

            int.TryParse(companyIdClaim.Value, out int companyId);
            return companyId;
        }
        public static int UserId(this IPrincipal principal)
        {
            var claimPrincipal = principal as ClaimsPrincipal;

            var identityName = claimPrincipal?.Identity.Name;

            if (identityName == null)
            {
                throw new Exception("Invalid User");
            }

            int.TryParse(identityName, out var userId);
            return userId;
        }

        public static int? ParentId(this IPrincipal principal)
        {

            var claimPrincipal = principal as ClaimsPrincipal;

            var parentIdClaim = claimPrincipal?.Claims.FirstOrDefault(c => c.Type == "parent_user_id");

            if (parentIdClaim == null)
            {
                return null;
            }

            int.TryParse(parentIdClaim.Value, out var parentId);
            return parentId;
        }
    }
}