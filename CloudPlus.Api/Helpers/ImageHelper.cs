using System;
using System.IO;
using System.Web.Hosting;
using MassTransit;

namespace CloudPlus.Api.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public string SaveProfilePicture(string base64String)
        {
            return SaveImage(base64String, ImageType.ProfilePicture);
        }

        public string SaveCompanyLogo(string base64String)
        {
            return SaveImage(base64String, ImageType.CompanyLogo);
        }

        private static string SaveImage(string base64String, ImageType imageType)
        {
            var imageName = $"{NewId.NextGuid().ToString().Replace("-", "").ToLower()}.png";

            if (base64String != "")
            {
                var prepopulated = "prepopulatedPicture";
                if (base64String == "defaultPicture")
                    return "128x128.png";
                if (base64String.Contains(prepopulated))
                    return base64String.Substring(prepopulated.Length); 
            }

            if (string.IsNullOrWhiteSpace(base64String))
                return "";
            
            var bytes = Convert.FromBase64String(base64String);
            var path = HostingEnvironment.MapPath($@"~\Static\Images\{imageType.ToString()}\{imageName}");

            File.WriteAllBytes(path ?? throw new InvalidOperationException(), bytes);

            return imageName;
        }
    }
    internal enum ImageType
    {
        ProfilePicture,
        CompanyLogo
    }
}