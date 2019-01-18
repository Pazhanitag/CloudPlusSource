namespace CloudPlus.Models.Office365.Transition
{
    public class Office365TransitionBasicMatchingDataModel
    {
        public string TenantId { get; set; }
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public bool ExistsInControlPanel { get; set; }
        public bool UnsupportedLicensesFound { get; set; }
        public string CurrentProducts { get; set; }
        public string RecommendedProduct { get; set; }
        public string RecommendedProductName { get; set; }
        public string RecommendedProductOfferId { get; set; }
        public double ProductMatchPercentage { get; set; }
    }
}
