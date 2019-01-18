namespace CloudPlus.Models.WorkflowActivity
{
    public class WorkflowActivityDto
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public WorkflowActivityContentDto Context { get; set; }
    }
}