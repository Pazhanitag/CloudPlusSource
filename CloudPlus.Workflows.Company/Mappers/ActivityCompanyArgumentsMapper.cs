using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Companies.Commands;

namespace CloudPlus.Workflows.Company.Mappers
{
    public class ActivityCompanyArgumentsMapper : IActivityCompanyArgumentsMapper
    {
        public dynamic MapActiveDirectoryCompanyArguments(ICreateCompanyCommand src)
        {
            var dest = new
            {
                src.Company,
                WorkflowActivityType = WorkflowActivityType.CreateCompany,
                WorkflowStep = WorkflowActivityStep.CreateActiveDirectoryCompany
            };

            return dest;
        }

        public dynamic MapDatabaseCompanyArguments(ICreateCompanyCommand src)
        {
            var dest = new
            {
                src.Company,
                src.Password,
                src.PasswordSetupMethod,
                src.PasswordSetupEmail,
                WorkflowActivityType = WorkflowActivityType.CreateCompany,
                WorkflowStep = WorkflowActivityStep.CreateDatabaseCompany
            };

            return dest;
        }

        public dynamic MapDatabaseCompanyArguments(IUpdateCompanyCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.Name,
                src.Website,
                src.LogoBase64,
                src.Logo,
                src.SupportSiteUrl,
                src.Email,
                src.PhoneNumber,
                src.StreetAddress,
                src.City,
                src.ZipCode,
                src.State,
                src.Country,
                src.BrandColorPrimary,
                src.BrandColorSecondary,
                src.BrandColorText,
                src.Domains,
                WorkflowActivityType = WorkflowActivityType.UpdateCompany,
                WorkflowStep = WorkflowActivityStep.UpdateDatabaseCompany
            };

            return dest;
        }

        public dynamic MapCreatedComapnySendEmailArguments(ICreateCompanyCommand src)
        {
            var dest = new
            {
                src.Company,
                src.Email,
                src.AlternativeEmail,
                src.Password,
                src.PasswordSetupMethod,
                src.SendPlainPasswordViaEmail,
                src.PasswordSetupEmail,
                src.SendWelcomeLetters
            };

            return dest;
        }

        public dynamic MapAssignCatalogArguments(ICreateCompanyCommand src)
        {
            var dest = new
            {
                src.Company.ParentId,
                src.Company.CatalogId
            };
            return dest;
        }
    }
}
