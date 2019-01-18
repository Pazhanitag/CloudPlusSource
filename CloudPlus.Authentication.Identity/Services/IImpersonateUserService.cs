using IdentityServer3.Core.Models;
using System.Threading.Tasks;

namespace CloudPlus.Authentication.Identity.Services
{
    public interface IImpersonateUserService
    {
        Task<AuthenticateResult> ImpersonateUser(string acrValue);
        bool IsImpersonateFlow(string acrValue);
    }
}
