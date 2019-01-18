using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Recaptcha
{
	public class HasRecaptchaPassedViewModel
	{
		[Required]
		public string Response { get; set; }
		public string Secret { get; set; }
		public string SiteKey { get; set; }
		public string RemoteIp { get; set; }
	}
}