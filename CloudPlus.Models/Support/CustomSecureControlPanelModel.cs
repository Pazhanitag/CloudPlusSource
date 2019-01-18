using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Support
{
    public class CustomSecureControlPanelModel
    {
        public int Id
        { get; set; }
        public string CustomSecureControlPanelURL { get; set; }
        public string CompanyName { get; set; }
        public string Email
        {
            get;
            set;
        }
        public string CompanyAddressStreet { get; set; }
        public string CompanyAddressCity { get; set; }
        public string CompanyAddressState { get; set; }
        public string CompanyAddressZipCode { get; set; }
        public string CompanyAddressCountry { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public bool IsDeleted
        { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //[DefaultValue("getdate()")]
        //public DateTime CreateDate
        //{ get; set; }
        ////[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        ////[DefaultValue("getdate()")]
        //public DateTime UpdateDate
        //{ get; set; }

    }
}
