using System.Threading.Tasks;
using MassTransit;
using CloudPlus.Models.Office365.User;
using CloudPlus.QueueModels.Office365.User;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.Office365.Customer;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365GetUserRolesConsumer : IOffice365GetUserRolesConsumer
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365ApiService _office365ApiService;

        public Office365GetUserRolesConsumer(
            IOffice365ApiService office365ApiService, 
            IOffice365DbCustomerService office365DbCustomerService)
        {
            _office365ApiService = office365ApiService;
            _office365DbCustomerService = office365DbCustomerService;
        }

        public async Task Consume(ConsumeContext<IOffice365GetUserRolesRequest> context)
        {
            var messages = context.Message;

            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(messages.CompanyId);
            
            var roles = await _office365ApiService.GetUserRoles(new Office365UserRolesModel
            {
                UserPrincipalName = messages.UserPrincipalName,
                Office365CustomerId = office365Customer.Office365CustomerId
            });

            context.Respond(new Office365GetUserRolesResponse
            {
                UserRoles = roles
            });
        }
    }
}
