namespace CloudPlus.Models.Office365.Transition
{
    public class Office365TransitionProductItemModel
    {
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string CurrentProductItemName { get; set; }
        public Office365TransitionRecommendedProductItemModel RecommendedProductItem { get; set; }
        public bool UnsupportedLicensesFound { get; set; }
        public bool Delete { get; set; }
        public bool RemoveLicenses { get; set; }
        public bool KeepLicenses { get; set; }
        public bool Admin { get; set; }
    }
}
