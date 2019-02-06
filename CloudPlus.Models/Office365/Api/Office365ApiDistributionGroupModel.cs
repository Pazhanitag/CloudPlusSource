

namespace CloudPlus.Models.Office365.Api
{
    public class Office365ApiDistributionGroupModel
    {
        public string DistributionGroupName { get; set; }
        public string ManagerSMTPAddress { get; set; }
        public string MemberJoinPolicy { get; set; }
        public string GroupSMTPAddress { get; set; }

    }
}