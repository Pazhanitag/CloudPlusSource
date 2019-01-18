using CloudPlus.Database;
using CloudPlus.Entities;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Utilities;
using CloudPlus.Services.Database.Office365.Domain;
using CloudPlus.Services.Database.WorkflowActivity.Office365;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudPlus.Services.Database.Office365.Utilities
{
    public class Office365DbUtilitiesService : IOffice365DbUtilitiesService
    {
        private readonly IOffice365DbDomainService _office365DomainService;
        private readonly IWorkflowOffice365ActivityService _workflowOffice365ActivityService;
        private readonly CldpDbContext _dbContext;

        public Office365DbUtilitiesService(IOffice365DbDomainService office365DomainService, IWorkflowOffice365ActivityService workflowOffice365ActivityService, CldpDbContext dbContext)
        {
            _office365DomainService = office365DomainService;
            _workflowOffice365ActivityService = workflowOffice365ActivityService;
            _dbContext = dbContext;
        }

        public Office365ProvisioningStatus CheckProvisioningStatus(int companyId)
        {
            // TODO: add additional check to see if office 365 is enabled or disabled
            // At the moment we can't do that, as we can't know the Office 365 product Id at this point. At least not yet.
			// Until we have that ready, this check will be performed on the UI

            if (_office365DomainService.IsAnyDomainAdded(companyId))
            {
                return Office365ProvisioningStatus.Completed;
            }

            if (_workflowOffice365ActivityService.IsOffice365ProvisioningInProgress(companyId))
            {
                return Office365ProvisioningStatus.InProgress;
            }

            return Office365ProvisioningStatus.Enabled;
        }

        public Office365CompatibleMatrixModel GetOffice365CompatibleMatrix()
        {
            Office365CompatibleMatrixModel office365CompatibleMatrixModel = new Office365CompatibleMatrixModel();
            office365CompatibleMatrixModel.licenses = _dbContext.ProductItems.Where(x => x.Product.Id == 1).Select(row => new Office365Services
            {
                cloudPlusProductIdentifier = row.Identifier,
                offerName = row.Name

            }).ToList();

            var DBCompatibleService = _dbContext.Office365InCompatibleService.Select(row => new Office365CompatabileServices
            {
                ServiceId = row.ServiceId,
                CompapatibleID = row.CompatibleServiceId,
            }).ToList();

            var CompatibleMatrixResult = DBCompatibleService.Select(row => new Office365CompatabileServices
            {
                ServiceId = row.ServiceId,
                ServiceName = office365CompatibleMatrixModel.licenses.Where(x => x.cloudPlusProductIdentifier == row.ServiceId).Select(z => z.offerName).SingleOrDefault(),
                CompapatibleID = row.CompapatibleID,
                CompatibleName = office365CompatibleMatrixModel.licenses.Where(x => x.cloudPlusProductIdentifier == row.CompapatibleID).Select(z => z.offerName).SingleOrDefault(),
            });

            office365CompatibleMatrixModel.compatibleLicensesForEachLicense = office365CompatibleMatrixModel.licenses.Select(row => new Office365CompatabileMatrix
            {
                selectedLicense = new Office365Services
                {
                    cloudPlusProductIdentifier = row.cloudPlusProductIdentifier,
                    offerName = row.offerName,
                },
                compatibleLicenses = CompatibleMatrixResult.Where(x => x.ServiceId == row.cloudPlusProductIdentifier).Select(c => new Office365Services
                {
                    cloudPlusProductIdentifier = c.CompapatibleID,
                    offerName = c.CompatibleName
                }).ToList(),

            }).ToList();


            return office365CompatibleMatrixModel;
        }

        #region AddUpdate office 365 compatible matrix 
        public bool AddUpdateCompatibleMatrix(List<Office365CompatabileMatrix> lstOffice365CompatabileMatrix)
        {
            try
            {
                var AllCompatibleMatrix = _dbContext.Office365InCompatibleService.ToList();
                foreach (var item in lstOffice365CompatabileMatrix)
                {
                    if (item.compatibleLicenses != null && item.compatibleLicenses.Count > 0)
                    {
                        var LocalCompatibleService = item.compatibleLicenses.Select(x => x.cloudPlusProductIdentifier).ToList();
                        if (AllCompatibleMatrix != null && AllCompatibleMatrix.Count > 0)
                        {
                            var DBCompatibleService = AllCompatibleMatrix.Where(x => x.ServiceId == item.selectedLicense.cloudPlusProductIdentifier).Select(y => y.CompatibleServiceId).ToList();
                            if (DBCompatibleService != null && DBCompatibleService.Count > 0)
                            {
                                var RemoveService = DBCompatibleService.Except(LocalCompatibleService);
                                var AddService = LocalCompatibleService.Except(DBCompatibleService);

                                if (AddService != null && AddService.Count() > 0)
                                {
                                    foreach (var assigned in AddService)
                                    {
                                        var CompatibleMat = new Office365InCompatibleService
                                        {
                                            IsDeleted = false,
                                            ServiceId = item.selectedLicense.cloudPlusProductIdentifier,
                                            CompatibleServiceId = assigned
                                        };
                                        _dbContext.Office365InCompatibleService.Add(CompatibleMat);
                                    }
                                }
                                if (RemoveService != null && RemoveService.Count() > 0)
                                {
                                    foreach (var assigned in RemoveService)
                                    {
                                        var Removeservice = _dbContext.Office365InCompatibleService.Where(x => x.CompatibleServiceId == assigned && x.ServiceId == item.selectedLicense.cloudPlusProductIdentifier).SingleOrDefault();
                                        _dbContext.Office365InCompatibleService.Remove(Removeservice);
                                    }
                                }
                            }
                            else {
                                foreach (var assigned in LocalCompatibleService)
                                {
                                    var CompatibleMat = new Office365InCompatibleService
                                    {
                                        IsDeleted = false,
                                        ServiceId = item.selectedLicense.cloudPlusProductIdentifier,
                                        CompatibleServiceId = assigned
                                    };
                                    _dbContext.Office365InCompatibleService.Add(CompatibleMat);
                                }
                            }
                        }
                        else
                        {
                            foreach (var assigned in LocalCompatibleService)
                            {
                                var CompatibleMat = new Office365InCompatibleService
                                {
                                    IsDeleted = false,
                                    ServiceId = item.selectedLicense.cloudPlusProductIdentifier,
                                    CompatibleServiceId = assigned
                                };
                                _dbContext.Office365InCompatibleService.Add(CompatibleMat);
                            }
                        }
                    }
                    else if (item.compatibleLicenses.Count == 0)
                    {
                        var Removeservice = _dbContext.Office365InCompatibleService.Where(x => x.ServiceId == item.selectedLicense.cloudPlusProductIdentifier).ToList();
                        foreach (var RemoveItem in Removeservice)
                        {
                            _dbContext.Office365InCompatibleService.Remove(RemoveItem);
                        }
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex) { }
            return true;
        }

       
        #endregion
    }
}
