using System;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.ViewModels.Request.Recaptcha;
using CloudPlus.Api.ViewModels.Response.Recaptcha;
using CloudPlus.Constants;
using CloudPlus.Resources;
using CloudPlus.Settings;

namespace CloudPlus.Api.Controllers.Utilities
{
	[RoutePrefix("api/recaptcha")]
	public class RecaptchaUtilitiesController : ApiController
    {
	    private readonly ICloudPlusApiSettings _cloudPlusApiSettings;

		public RecaptchaUtilitiesController(ICloudPlusApiSettings cloudPlusApiSettings)
	    {
		    _cloudPlusApiSettings = cloudPlusApiSettings;
	    }

		[HttpPost]
		[Route("", Name = "HasReCaptchaPassed")]
	    public IHttpActionResult HasReCaptchaPassed(HasRecaptchaPassedViewModel clientResponse)
	    {
		    var googleClient = new System.Net.WebClient();
		    var secret = _cloudPlusApiSettings.GoogleRecaptchaSecretKey;
		    var encodedResponse = clientResponse.Response;

			var googleReply = googleClient.DownloadString(
				String.Format(_cloudPlusApiSettings.GoogleRecaptchaAPIUri, secret, encodedResponse)
		    );

			var serializer = new JsonSerializer();
		    RecaptchaViewModel response = serializer.Deserialize<RecaptchaViewModel>(googleReply);

			return Ok(response);
		}
	}
}
