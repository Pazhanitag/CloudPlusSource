namespace CloudPlus.Models.WorkflowActivity
{
    public class WorkflowActivityExceptionDto
    {
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string InnerMessage { get; set; }
    }
}