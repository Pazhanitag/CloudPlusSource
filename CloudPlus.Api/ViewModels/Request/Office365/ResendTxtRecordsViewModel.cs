namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class ResendTxtRecordsViewModel
    {
        public string Domain { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
    }
}