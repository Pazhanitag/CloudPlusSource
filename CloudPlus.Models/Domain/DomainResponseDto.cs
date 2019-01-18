using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Domain
{
    public class DomainResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrimary { get; set; }
        public int CompanyId { get; set; }
    }
}
