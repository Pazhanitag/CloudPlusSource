namespace CloudPlus.Workflows.Office365.Activities.Domain.SendDomainVerifiedEmail
{
    public interface ISendDomainVerifiedEmailArguments
    {
        string Email { get; set; }
        int CompanyId { get; set; }
        string Domain { get; set; }
        bool IsDomainPrimary { get; set; }
    }
}
