using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Domain
{
    public class CreateDomainRequestDto
    {
        [Required]
        public string Name { get; set; }
        public bool IsPrimary { get; set; }
    }
}
