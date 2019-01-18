using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CloudPlus.Authentication.Api.Certificate
{
    public class Certificate
    {
        public static X509Certificate2 Load()
        {
            var assembly = typeof(Certificate).Assembly;
            using (var stream = assembly.GetManifestResourceStream("CloudPlus.Authentication.Api.Certificate.CloudPlusIdentity.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "C!oud+Plu5");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}