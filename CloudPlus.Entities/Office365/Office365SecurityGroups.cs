﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities.Office365
{
    public class Office365SecurityGroup : IBaseEntity
    {
        public Office365SecurityGroup()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
            
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Office365GroupId { get; set; }
        public string Office365GroupName { get; set; }
        public string UserPrincipalName { get; set; }

    }
}
