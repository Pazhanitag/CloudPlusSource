using System.Threading.Tasks;

namespace CloudPlus.Services.Office365.Utilities
{
    public interface IOffice365UtilitiesService
    {
        Task<string> GenerateDefaultMicrosoftDomainAsync(int companyOu);
    }
}
