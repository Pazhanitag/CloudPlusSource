using Microsoft.Store.PartnerCenter;

namespace CloudPlus.Services.Office365.Operations
{
    public interface IPartnerOperations
    {
        IAggregatePartner UserPartnerOperations { get; }
    }
}
