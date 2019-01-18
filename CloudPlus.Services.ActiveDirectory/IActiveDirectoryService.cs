using System.Threading.Tasks;
using CloudPlus.Services.ActiveDirectory.Models;

namespace CloudPlus.Services.ActiveDirectory
{
    public interface IActiveDirectoryService
    {
	    /// <summary>
	    /// Checks if user exists on AD. If it does, it returns the user object and status Ok.
	    /// If the user does not exist it return status NotFound
	    /// </summary>
	    /// <param name="user"></param>
	    Task<bool> UserExists(ActiveDirectoryUser user);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		Task CreateUser(ActiveDirectoryUser user);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		Task Update(ActiveDirectoryUser user);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="upn">User principal name (email address)</param>
		Task DeleteUser(string upn);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="upn">User principal name (email address)</param>
		/// <param name="newPassword"></param>
		Task UpdateUserPassword(string upn, string newPassword);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="company"></param>
		Task CreateCompany(ActiveDirectoryCompany company);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="companyOu"></param>
		Task DeleteCompany(int companyOu);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<int> GenerateCompanyOuIdAsync();
    }
}