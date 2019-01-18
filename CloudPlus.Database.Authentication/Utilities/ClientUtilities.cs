using CloudPlus.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Database.Authentication.Utilities
{
    public class ClientUtilities
    {
        private readonly CloudPlusAuthDbContext _context;
        private readonly IConfigurationManager _configurationManager;

        public ClientUtilities(CloudPlusAuthDbContext context, IConfigurationManager configurationManager)
        {
            _context = context;
            _configurationManager = configurationManager;
        }

        public ClientUtilities UpdateTokenSettings()
        {
            var clientRedirectUrl = _context.ClientRedirectUri.FirstOrDefault(redirect => redirect.Uri.Contains("silent"));

            if(clientRedirectUrl != null)
            {
                clientRedirectUrl.Uri = _configurationManager.GetByKey("AdminPortal.SilentRedirectUri");

                _context.ClientRedirectUri.Attach(clientRedirectUrl);
                _context.Entry(clientRedirectUrl).State = EntityState.Modified;

                _context.SaveChanges();
            }

            return this;
        }
    }
}
