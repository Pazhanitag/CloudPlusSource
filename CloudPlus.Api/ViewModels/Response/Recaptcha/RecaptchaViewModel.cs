using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CloudPlus.Api.ViewModels.Response.Recaptcha
{
	public class RecaptchaViewModel
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("error-codes")]
		public IEnumerable<string> ErrorCodes { get; set; }

		[JsonProperty("challenge_ts")]
		public DateTime ChallengeTs { get; set; }

		[JsonProperty("hostname")]
		public string Hostname { get; set; }
	}
}