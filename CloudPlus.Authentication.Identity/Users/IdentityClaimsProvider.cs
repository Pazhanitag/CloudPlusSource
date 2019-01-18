using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Validation;

namespace CloudPlus.Authentication.Identity.Users
{
    public class IdentityClaimsProvider: DefaultClaimsProvider
    {
        public IdentityClaimsProvider(IUserService users) : base(users)
        { }

        public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client, IEnumerable<Scope> scopes, ValidatedRequest request)
        {
            var claims = await base.GetAccessTokenClaimsAsync(subject, client, scopes, request);

            var customClaims = claims.ToList();

            var companyIdClaim = subject?.FindFirst("company_id");
            var paretnUserIdClaim = subject?.FindFirst("parent_user_id");

            if (companyIdClaim != null)
            {
                customClaims.Add(companyIdClaim);
            }
            if (paretnUserIdClaim != null)
            {
                customClaims.Add(paretnUserIdClaim);
            }
            return customClaims;
        }
    }
}
