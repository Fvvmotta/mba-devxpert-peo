using System.Drawing;

namespace MBA_DevXpert_PEO.Api.Identity
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }  
        public string RootFilePath { get; set; }
        public string FofFilePath { get; set; }
        public string ServerUrl { get; set; }
    }

}