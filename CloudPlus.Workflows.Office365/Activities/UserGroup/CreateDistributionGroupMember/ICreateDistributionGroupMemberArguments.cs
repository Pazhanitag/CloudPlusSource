
namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroupMember
{
    public interface ICreateDistributionGroupMemberArguments
    {
        string DistributionGroupName { get; set; }
        string MemberSMTPAddress { get; set; }
    }
}
