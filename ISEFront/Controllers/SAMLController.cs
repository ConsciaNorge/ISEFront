using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Utility;

using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Linq;
using CiscoISE.Models;
using System;

namespace ISEFront.Controllers
{
    // TODO: This has to go!!! Stop using WebConfigurationManager
    public static class AppSettings
    {
        public const string Attribute = "Attribute";
        public const string PartnerSP = "PartnerSP";
        public const string SubjectName = "SubjectName";
        public const string TargetUrl = "TargetUrl";
    }

    public class SAMLController : Controller
    {
        private const string ssoPendingSessionKey = "ssoPending";

        public async Task<ActionResult> SSOService()
        {
            var logger = log4net.LogManager.GetLogger("SAML");

            logger.Info("SSOService() - Single sign-on service entered");

            var pendingSessionKey = Session[ssoPendingSessionKey];
            logger.Debug("Session[ssoPendingSessionKey] = " + ((pendingSessionKey == null) ? "null" : pendingSessionKey.ToString()));

            // Either an authn request has been received or login has just completed in response to a previous authn request.
            // The SSO pending session flag is false if an authn request is expected. Otherwise, it is true if
            // a login has just completed and control is being returned to this page.
            bool ssoPending = pendingSessionKey != null && (bool)Session[ssoPendingSessionKey] == true;

            logger.Debug("ssoPending = " + ssoPending.ToString());
            logger.Debug("User.Identity.IsAuthenticated = " + User.Identity.IsAuthenticated.ToString());
            if (!(ssoPending && User.Identity.IsAuthenticated))
            {
                string partnerSP = null;

                // Receive the authn request from the service provider (SP-initiated SSO).
                SAMLIdentityProvider.ReceiveSSO(Request, out partnerSP);
                logger.Debug("partnerSP = " + partnerSP);

                // If the user isn't logged in at the identity provider, force the user to login.
                if (!User.Identity.IsAuthenticated)
                {
                    Session[ssoPendingSessionKey] = true;
                    logger.Info("User not authenticated, redirecting to login page()");

                    FormsAuthentication.RedirectToLoginPage();
                    return new EmptyResult();
                }
            }

            Session[ssoPendingSessionKey] = null;

            // The user is logged in at the identity provider.
            // Respond to the authn request by sending a SAML response containing a SAML assertion to the SP.
            // Use the configured or logged in user name as the user name to send to the service provider (SP).
            // Include some user attributes.
            string userName = WebConfigurationManager.AppSettings[AppSettings.SubjectName];

            if (string.IsNullOrEmpty(userName))
            {
                var identity = (ClaimsIdentity)User.Identity;

                var bid = identity.FindFirst("bid_id");
                if (bid == null)
                {
                    logger.Info("bid_id claim not found, redirecting to login page");
                    FormsAuthentication.RedirectToLoginPage();
                    return new EmptyResult();
                }

                userName = bid.Value;

                var firstName = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                var lastName = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");

                logger.Info("Username obtained from BankID : " + userName);
                logger.Info("Firstname from BankID : " + ((firstName == null) ? "<not provided>" : firstName.Value));
                logger.Info("Lastname from BankID : " + ((lastName == null) ? "<not provided>" : lastName.Value));

                var iseSettings = Utility.Configuration.Settings.IseServer;

                var iseUri = new UriBuilder("https", iseSettings.ServerIP.ToString(), 9060);

                var developerConnection = new CiscoISE.ISEConnection(iseUri.Uri, iseSettings.ApiUsername, iseSettings.ApiPassword);

                var portals = await CiscoISE.Portals.Get(developerConnection);
                if (portals == null)
                    throw new Exception("Could not access ISE to enumerate portals");

                logger.Debug("Retrieved portals from the Cisco ISE server using the API account");

                // TODO : Research whether this is the best way to get the sponsor portal.
                var sponsorPortal = portals.Where(x => x.Name == "Sponsor Portal (default)").FirstOrDefault();
                if (sponsorPortal == null)
                    throw new Exception("Could not access ISE to find GUID for Sponsor Portal (default)");

                logger.Debug("Sponsor portal found on ISE server : " + sponsorPortal.Name);

                var sponsorConnection = new CiscoISE.ISEConnection(iseUri.Uri, iseSettings.SponsorPortalUsername, iseSettings.SponsorPortalPassword);

                logger.Debug("Finding an existing guest user with username : " + userName);
                var guestUser = await CiscoISE.GuestUsers.Get(sponsorConnection, userName);
                if (guestUser == null)
                {
                    logger.Info("Existing user (" + userName + ") not found, attempt to create a new one");

                    logger.Debug("Getting GuestTypes from ISE server");
                    // TODO : Catch errors on connecting to the API
                    var guestTypes = await CiscoISE.GuestTypes.Get(developerConnection);
                    if(guestTypes == null)
                    {
                        // TODO : Provide a better method of handling API issues
                        throw new Exception("Failed to get a list of GuestTypes from Cisco ISE");
                    }

                    // TODO : Provide a configuration option to allow specific guest types to be
                    //          chosen based on how the user is logging in.
                    var guestType = guestTypes.FirstOrDefault();
                    if(guestType == null)
                    {
                        // TODO : Provide a better method of dealing with there not being any guest types
                        throw new Exception("There appears to be no guest types configured in Cisco ISE");
                    }

                    logger.Info("Using first guest type in the list : " + guestType.Name);

                    logger.Debug("Getting list of guest locations from the ISE server");
                    // TODO : Catch exceptions on guest locations
                    var guestLocations = await CiscoISE.GuestLocations.Get(developerConnection);
                    if(guestLocations == null)
                    {
                        // TODO : Provide a better method of handling API issues
                        throw new Exception("Failed to obtain a list of guest locations from Cisco ISE");
                    }

                    // TODO : Provide a configuration option to allow specific guest locations to be
                    //         chosen based on where the user is logging in
                    var guestLocation = guestLocations.FirstOrDefault();
                    if(guestLocation == null)
                    {
                        // TODO : Provide a better method of handling API issues
                        throw new Exception("ISE does not appear to have any guest locations configured");
                    }

                    logger.Info("Using first guest location from the list " + guestLocation.Name);

                    var randomPassword = Membership.GeneratePassword(16, 4);

                    // TODO : Consider removing this from the code. Though, the password is not used anywhere, it could be a "alarm" during a code audit
                    logger.Info("Automatically generated a password for the new user (" + randomPassword + ")");

                    guestUser = new GuestUserViewModel
                    {
                        GuestType = guestType.Name, // TODO : File "bug" with Cisco over name referencing instead of ID
                        PortalId = sponsorPortal.Id.ToString(),
                        GuestInfo = new CiscoISE.GuestInfoViewModel
                        {
                            Username = userName,
                            Password = randomPassword,
                            FirstName = firstName == null ? "<unknown>" : firstName.Value,
                            LastName = lastName == null ? "<unknown>" : lastName.Value,
                            Enabled = true
                        },
                        GuestAccessInfo = new GuestAccessInfoViewModel
                        {
                            ValidDays = 100,
                            Location = guestLocation.Name // TODO : File "bug" with Cisco over name reference instead of ID
                        }
                    };
                    logger.Debug("guestUser = " + Newtonsoft.Json.JsonConvert.SerializeObject(guestUser));

                    logger.Info("Creating new guest user");
                    var created = await CiscoISE.GuestUsers.Create(
                            sponsorConnection,
                            guestUser
                        );

                    logger.Info(created ? "Guest user created" : "Guest user failed to be created");
                    if (!created)
                        throw new Exception("Failed to create new guest user, cannot continue login process");
                }
            }

            logger.Info("Appending attributes to SAML assertion");
            IDictionary<string, string> attributes = new Dictionary<string, string>();
            foreach (string key in WebConfigurationManager.AppSettings.Keys)
            {
                if (key.StartsWith(AppSettings.Attribute))
                {
                    attributes[key.Substring(AppSettings.Attribute.Length + 1)] = WebConfigurationManager.AppSettings[key];
                }
            }

            //ComponentSpace.SAML2.SAMLController.TraceLevel = System.Diagnostics.TraceLevel.Verbose;
            logger.Info("Sending SSO to SAML service provider");
            SAMLIdentityProvider.SendSSO(Response, userName, attributes);

            return new EmptyResult();
        }

        public ActionResult SLOService()
        {
            // Receive the single logout request or response.
            // If a request is received then single logout is being initiated by the service provider.
            // If a response is received then this is in response to single logout having been initiated by the identity provider.
            bool isRequest = false;
            bool hasCompleted = false;
            string logoutReason = null;
            string partnerSP = null;
            string relayState = null;

            SAMLIdentityProvider.ReceiveSLO(Request, Response, out isRequest, out hasCompleted, out logoutReason, out partnerSP, out relayState);

            if (isRequest)
            {
                // Logout locally.
                FormsAuthentication.SignOut();

                // Respond to the SP-initiated SLO request indicating successful logout.
                SAMLIdentityProvider.SendSLO(Response, null);
            }
            else
            {
                if (hasCompleted)
                {
                    // IdP-initiated SLO has completed.
                    Response.Redirect("~/");
                }
            }

            return new EmptyResult();
        }

        public ActionResult ECP()
        {
            // Receive an authn request from an enhanced client or proxy (ECP).
            string partnerSP = null;

            SAMLIdentityProvider.ReceiveSSO(Request, out partnerSP);

            // In this example, the user's credentials are assumed to be included in the HTTP authorization header.
            // The application should authenticate the user against some user registry.
            // In this example, the credentials are assumed to be valid and no check is made.
            string userName = null;
            string password = null;

            HttpBasicAuthentication.GetAuthorizationHeader(Request, out userName, out password);

            // Respond to the authn request by sending a SAML response containing a SAML assertion to the SP.
            // Use the configured or logged in user name as the user name to send to the service provider (SP).
            // Include some user attributes.
            if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings[AppSettings.SubjectName]))
            {
                userName = WebConfigurationManager.AppSettings[AppSettings.SubjectName];
            }

            IDictionary<string, string> attributes = new Dictionary<string, string>();

            foreach (string key in WebConfigurationManager.AppSettings.Keys)
            {
                if (key.StartsWith(AppSettings.Attribute))
                {
                    attributes[key.Substring(AppSettings.Attribute.Length + 1)] = WebConfigurationManager.AppSettings[key];
                }
            }

            SAMLIdentityProvider.SendSSO(Response, userName, attributes);

            return new EmptyResult();
        }
    }
}
