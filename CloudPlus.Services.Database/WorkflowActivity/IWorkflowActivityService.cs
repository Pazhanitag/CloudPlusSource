using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CloudPlus.Models.WorkflowActivity;

namespace CloudPlus.Services.Database.WorkflowActivity
{
    public interface IWorkflowActivityService
    {
        void Insert(WorkflowActivityDto workflowActivityDto);
        IEnumerable<WorkflowActivityDto> Get(string column, object value);
        IEnumerable<WorkflowActivityDto> Get(Expression<Func<Entities.WorkflowActivity, bool>> expression);
        IEnumerable<WorkflowActivityDto> Get(Dictionary<string, object> parameters);
    }
}