using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities
{
    public class CustomSecureControlPanel : IBaseEntity
    {
        public CustomSecureControlPanel()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string CustomSecureControlPanelURL { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string CompanyAddressStreet { get; set; }
        public string CompanyAddressCity { get; set; }
        public string CompanyAddressState { get; set; }
        public string CompanyAddressZipCode { get; set; }
        public string CompanyAddressCountry { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int StatusId { get; set; }
        public CustomSecureControlPanelStatus Status { get; set; }
    }
}
