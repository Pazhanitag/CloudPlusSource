using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Price
{
    public class EditGeneralInformationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> Companies { get; set; }
    }
}
