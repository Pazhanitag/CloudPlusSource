using System;

namespace CloudPlus.QueueModels
{
    public interface IQueueModel
    {
        string UniqueId { get; set; }
        bool IsStart { get; set; }
        bool IsEnd { get; set; }
        DateTime Created { get; set; }
        DateTime Completed { get; set; }
    }
}