using ISEFront.api.ViewModels;
using ISEFront.Utility.Configuration;
using System.Web.Http;

namespace ISEFront.api
{
    public class BankIDController : ApiController
    {
        [HttpGet]
        [Route("api/bankid/settings")]
        public BankIDSettingsViewModel GetSettings()
        {
            return Settings.BankID;
        }

        [HttpPut]
        [Route("api/bankid/settings")]
        public BankIDSettingsViewModel PutSettings(BankIDSettingsViewModel settings)
        {
            Settings.BankID = settings;

            return GetSettings();
        }
    }
}