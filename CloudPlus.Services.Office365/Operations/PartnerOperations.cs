using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter;
using Microsoft.Store.PartnerCenter.Extensions;
using CloudPlus.Settings;

namespace CloudPlus.Services.Office365.Operations
{
    public class PartnerOperations : IPartnerOperations
    {
        private readonly IOffice365ServiceSettings _office365ServiceSettings;
        private IAggregatePartner _userPartnerOperations;

        public PartnerOperations(IOffice365ServiceSettings office365ServiceSettings)
        {
            _office365ServiceSettings = office365ServiceSettings;
        }

        //public IAggregatePartner UserPartnerOperations
        //{
        //    get
        //    {
        //        if (_userPartnerOperations != null)
        //            return _userPartnerOperations;

        //        var aadAuthenticationResult = LoginUserToAad();

        //        var userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
        //            _office365ServiceSettings.ApplicationId,
        //            new AuthenticationToken(
        //                aadAuthenticationResult.AccessToken,
        //                aadAuthenticationResult.ExpiresOn),
        //            delegate
        //            {
        //                var aadToken = LoginUserToAad();

        //                return Task.FromResult(new AuthenticationToken(aadToken.AccessToken, aadToken.ExpiresOn));
        //            });

        //        _userPartnerOperations = PartnerService.Instance.CreatePartnerOperations(userCredentials);

        //        return _userPartnerOperations;
        //    }
        //}

        public IAggregatePartner UserPartnerOperations
        {
            get
            {
                if (_userPartnerOperations != null)
                    return _userPartnerOperations;

                #region "App + User Authenication"

                // Commented out this code, as this approach requires MFA /

                //var aadAuthenticationResult = LoginUserToAad();

                //var userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                //    _office365ServiceSettings.ApplicationId,
                //    new AuthenticationToken(
                //        aadAuthenticationResult.AccessToken,
                //        aadAuthenticationResult.ExpiresOn),
                //    delegate
                //    {
                //        var aadToken = LoginUserToAad();

                //        return Task.FromResult(new AuthenticationToken(aadToken.AccessToken, aadToken.ExpiresOn));
                //    });

                //_userPartnerOperations = PartnerService.Instance.CreatePartnerOperations(userCredentials);

                #endregion

                #region "App only authenication"

                IPartnerCredentials partnerCredentials = PartnerCredentials.Instance.GenerateByApplicationCredentials(
                                                        _office365ServiceSettings.ApplicationId,
                                                        _office365ServiceSettings.ApplicationSecret,
                                                        _office365ServiceSettings.Domain);

                _userPartnerOperations = PartnerService.Instance.CreatePartnerOperations(partnerCredentials);

                #endregion

                return _userPartnerOperations;
            }
        }
        private AuthenticationResult LoginUserToAad()
        {
            var addAuthority = new UriBuilder(_office365ServiceSettings.AuthenticationAuthorityEndpoint)
            {
                Path = _office365ServiceSettings.CommonDomain
            };

            var userCredentials = new UserCredential(
                _office365ServiceSettings.UserName,
                _office365ServiceSettings.Password);

            var authContext = new AuthenticationContext(addAuthority.Uri.AbsoluteUri);

            return authContext.AcquireToken(
                _office365ServiceSettings.ResourceUrl,
                _office365ServiceSettings.ApplicationId,
                userCredentials);
        }


    }
}
