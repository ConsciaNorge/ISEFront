using CiscoISE;
using CiscoISE.Models;
using ISEFront.api.ViewModels;
using ISEFront.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISEFront.api
{
    public class CiscoISEController : ApiController
    {
        [HttpGet]
        [Route("api/ciscoise/iseserversettings")]
        public ISEServerSettingsViewModel GetIseServerSettings()
        {
            return Settings.IseServer;
        }

        [HttpPut]
        [Route("api/ciscoise/iseserversettings")]
        public ISEServerSettingsViewModel PutIseServerSettings(ISEServerSettingsViewModel settings)
        {
            Settings.IseServer = settings;
            return settings;
        }

        [HttpGet]
        [Route("api/ciscoise/portals")]
        public async Task<List<PortalBriefViewModel>> GetIsePortals()
        {
            // TODO : Return a proper http response code

            var connection = GetIseConnection();
            if (connection == null)
                return null;

            var portals = await Portals.Get(connection);

            return portals;
        }

        [HttpGet]
        [Route("api/ciscoise/portal/{id}")]
        public async Task<PortalViewModel> GetIsePortal(Guid id)
        {
            // TODO : Return a proper http response code

            var connection = GetIseConnection();
            if (connection == null)
                return null;

            var portal = await Portals.Get(connection, id);

            return portal;
        }

        [HttpGet]
        [Route("api/ciscoise/portal/{portalId}/guestusers")]
        public async Task<List<GuestUserBriefViewModel>> GetIsePortalGuestUsers(Guid portalId)
        {
            // TODO : Return a proper http response code

            var connection = GetIseSponsorConnection();
            if (connection == null)
                return null;

            var guestUsers = await GuestUsers.Get(connection);

            return guestUsers;
        }

        [HttpGet]
        [Route("api/ciscoise/portal/{portalId}/guestusers/detailed")]
        public async Task<List<GuestUserViewModel>> GetIsePortalGuestUsersDetailed(Guid portalId)
        {
            // TODO : Return a proper http response code

            var connection = GetIseSponsorConnection();
            if (connection == null)
                return null;

            var guestUsers = await GuestUsers.Get(connection);
            var result = new List<GuestUserViewModel>();

            // TODO : use batch request
            foreach(var guestUser in guestUsers)
            {
                var detailedUser = await GuestUsers.Get(connection, guestUser);
                if(detailedUser == null)
                {
                    // TODO : Handle this more gracefully or create a decent exception instead.
                    throw new Exception("Failed to get detailed user information for " + guestUser.Name);
                }

                result.Add(detailedUser);
            }

            return result;
        }

        private static ISEConnection GetIseConnection()
        {
            var serverSettings = Settings.IseServer;

            // TODO : Create a helper function for validating server settings model
            if (
                serverSettings == null ||
                serverSettings.ServerIP == null ||
                serverSettings.ApiUsername == null ||
                serverSettings.ApiUsername == "" ||
                serverSettings.ApiPassword == null ||
                serverSettings.ApiPassword == ""
                )
                return null;
            var builder = new UriBuilder("https", serverSettings.ServerIP.ToString(), 9060);

            return new ISEConnection(
                builder.Uri,
                serverSettings.ApiUsername,
                serverSettings.ApiPassword
                );
        }

        private static ISEConnection GetIseSponsorConnection()
        {
            var serverSettings = Settings.IseServer;

            // TODO : Create a helper function for validating server settings model
            if (
                serverSettings == null ||
                serverSettings.ServerIP == null ||
                serverSettings.SponsorPortalUsername == null ||
                serverSettings.SponsorPortalUsername == "" ||
                serverSettings.SponsorPortalPassword == null ||
                serverSettings.SponsorPortalPassword == ""
                )
                return null;
            var builder = new UriBuilder("https", serverSettings.ServerIP.ToString(), 9060);

            return new ISEConnection(
                builder.Uri,
                serverSettings.SponsorPortalUsername,
                serverSettings.SponsorPortalPassword
                );
        }
    }
}