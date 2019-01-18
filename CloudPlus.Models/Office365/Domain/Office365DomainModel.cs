using CloudPlus.Enums.Office365;

namespace CloudPlus.Models.Office365.Domain
{
    public class Office365DomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFederated { get; set; }
        public bool VerificationInProgress { get; set; }
        public Office365DomainStatus Office365DomainStatus { get; set; }
    }
}
