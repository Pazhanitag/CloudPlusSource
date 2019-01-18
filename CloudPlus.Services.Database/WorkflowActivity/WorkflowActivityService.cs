using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Models.WorkflowActivity;
using CloudPlus.Resources;

namespace CloudPlus.Services.Database.WorkflowActivity
{
    public class WorkflowActivityService : IWorkflowActivityService
    {
        private readonly CldpDbContext _dbContext;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IJsonValueSqlBuilder _jsonValueSqlBuilder;
        public WorkflowActivityService(CldpDbContext dbContext, IObjectSerializer objectSerializer, IJsonValueSqlBuilder jsonValueSqlBuilder)
        {
            _dbContext = dbContext;
            _objectSerializer = objectSerializer;
            _jsonValueSqlBuilder = jsonValueSqlBuilder;
        }

        public void Insert(WorkflowActivityDto workflowActivityDto)
        {
            if(workflowActivityDto == null)
                throw new ArgumentNullException(nameof(workflowActivityDto));

            //_dbContext.WorkflowActivity.Add(new Entities.WorkflowActivity
            //{
            //    UniqueId = workflowActivityDto.UniqueId,
            //    Context = _objectSerializer.Serialize(workflowActivityDto.Context)
            //});

            //_dbContext.SaveChanges();

            //TODO This should be deleted, just testing purpose
            var context = new CldpDbContext();

            context.WorkflowActivity.Add(new Entities.WorkflowActivity
            {
                UniqueId = workflowActivityDto.UniqueId,
                Context = _objectSerializer.Serialize(workflowActivityDto.Context)
            });

            context.SaveChanges();
        }

        public IEnumerable<WorkflowActivityDto> Get(Expression<Func<Entities.WorkflowActivity, bool>> expression)
        {
            var workflowActivities = _dbContext.WorkflowActivity.AsNoTracking().Where(expression);

            var workflowActivityList = new List<WorkflowActivityDto>();

            workflowActivities.ToList()
                .ForEach(workflowActivity =>
                {
                    var context = _objectSerializer.Deserialize<WorkflowActivityContentDto>(workflowActivity.Context);

                    workflowActivityList.Add(new WorkflowActivityDto
                    {
                        Id = workflowActivity.Id,
                        UniqueId = workflowActivity.UniqueId,
                        Context = context
                    });
                });

            return workflowActivityList;
        }

        public IEnumerable<WorkflowActivityDto> Get(string column, object value)
        {
            var workflowActivities = _dbContext.WorkflowActivity
                .SqlQuery($"SELECT Id, UniqueId, Context, CreateDate, UpdateDate, IsDeleted FROM WorkflowActivities {_jsonValueSqlBuilder.Where(column, value).Build("Context")}")
                .AsNoTracking();

            var workflowActivityList = new List<WorkflowActivityDto>();

            workflowActivities.ToList()
                .ForEach(workflowActivity =>
                {
                    var context = _objectSerializer.Deserialize<WorkflowActivityContentDto>(workflowActivity.Context);

                    workflowActivityList.Add(new WorkflowActivityDto
                    {
                        Id = workflowActivity.Id,
                        UniqueId = workflowActivity.UniqueId,
                        Context = context
                    });
                });

            return workflowActivityList;
        }

        public IEnumerable<WorkflowActivityDto> Get(Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                _jsonValueSqlBuilder.And(parameter.Key, parameter.Value);
            }

            var workflowActivities = _dbContext.WorkflowActivity
                .SqlQuery("SELECT Id, UniqueId, Context, CreateDate, UpdateDate, IsDeleted FROM WorkflowActivities " +
                          _jsonValueSqlBuilder.Build("Context"))
                .AsNoTracking();

            var workflowActivityList = new List<WorkflowActivityDto>();

            workflowActivities.ToList()
                .ForEach(workflowActivity =>
                {
                    var context = _objectSerializer.Deserialize<WorkflowActivityContentDto>(workflowActivity.Context);

                    workflowActivityList.Add(new WorkflowActivityDto
                    {
                        Id = workflowActivity.Id,
                        UniqueId = workflowActivity.UniqueId,
                        Context = context
                    });
                });

            return workflowActivityList;
        }
    }
}