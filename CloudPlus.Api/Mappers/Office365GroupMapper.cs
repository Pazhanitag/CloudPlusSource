using CloudPlus.Api.ViewModels.Request.Office365;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Mappers
{
    public static class Office365SecurityGroupMapper
    {
        public static dynamic ToCreateSecurityGroupCommand(this Office365SecurityGroupViewModel securityGroup)
        {
            if (securityGroup == null)
                return null;
            return new 
            {
                securityGroup.SecurityGroupName,
                securityGroup.CompanyId 
            };
        }
    }
}