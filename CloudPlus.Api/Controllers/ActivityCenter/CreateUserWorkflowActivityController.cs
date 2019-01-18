using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CloudPlus.Models.WorkflowActivity;
using CloudPlus.Services.Database.WorkflowActivity;

namespace CloudPlus.Api.Controllers.ActivityCenter
{
    [RoutePrefix("api/activity/createuser")]
    public class CreateUserWorkflowActivityController : ApiController
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public CreateUserWorkflowActivityController(IWorkflowActivityService workflowActivityService)
        {
            _workflowActivityService = workflowActivityService;
        }

        [HttpGet]
        [Route("uniqueid/{uniqueId}", Name = "GetWorkflowActivityStatusById")]
        public IHttpActionResult GetStatusByUniqueId(string uniqueid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uniqueid))
                    throw new ArgumentNullException(nameof(uniqueid));

                var workflowActivityList = _workflowActivityService
                    .Get(workflowActivity =>
                        workflowActivity.UniqueId == uniqueid);

                if (!workflowActivityList?.Any() != true)
                    return NotFound();

                return Ok(workflowActivityList);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("company/{companyId:int}", Name = "GetWorkflowActivityStatusByCompanyId")]
        public IHttpActionResult GetStatusByCompanyId(int companyId)
        {
            try
            {
                var workflowActivityList = _workflowActivityService.Get("Data.companyId", companyId);

                if (!workflowActivityList?.Any() != true)
                    return NotFound();

                return Ok(GetWorkflowActivityStatus(workflowActivityList));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        private List<WorkflowActivityDto> GetWorkflowActivityStatus(IEnumerable<WorkflowActivityDto> workflowActivityList)
        {
            return workflowActivityList.GroupBy(group => group.UniqueId)
                .Select(groupActivity => _workflowActivityService.Get(activity => activity.UniqueId == groupActivity.Key)
                    .LastOrDefault())
                .ToList();
        }

        [HttpGet]
        [Route("upn/{upn}", Name = "GetWorkflowActivityStatusByUpn")]
        public IHttpActionResult GetStatusByUpnl(string upn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(upn))
                    throw new ArgumentNullException(nameof(upn));

                var workflowActivityList = _workflowActivityService.Get("Data.upn", upn);

                if (!workflowActivityList?.Any() != true)
                    return NotFound();

                return Ok(GetWorkflowActivityStatus(workflowActivityList));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("workflowType/{workflowType:int}", Name = "GetWorkflowActivityStatusByWorkflowType")]
        public IHttpActionResult GetStatusByWorkflowType(int workflowType)
        {
            try
            {
                var workflowActivityList = _workflowActivityService.Get("Data.workflowType", workflowType);

                if (!workflowActivityList?.Any() != true)
                    return NotFound();

                return Ok(GetWorkflowActivityStatus(workflowActivityList));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}