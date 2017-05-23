using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ISEFront.Models;
using ISEFront.Utility.Configuration;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;
using System.Net.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ISEFront
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            //// Enable the application to use a cookie to store information for the signed in user
            //// and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //// Configure the sign in cookie
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //            validateInterval: TimeSpan.FromMinutes(30),
            //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            //// Enables the application to remember the second login verification factor such as phone or email.
            //// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            //// This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            //app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            var bankIdSettings = Settings.BankID;
            if (bankIdSettings != null)
            {
                var options = new OpenIdConnectAuthenticationOptions
                {
                    ClientId = bankIdSettings.ClientId,
                    ClientSecret = bankIdSettings.ClientSecret,
                    Authority = bankIdSettings.OIDCBaseUrl.ToString(),
                    RedirectUri = bankIdSettings.RedirectUrl.ToString(),
                    MetadataAddress = bankIdSettings.ManifestUrl.ToString(),
                    ResponseType = "code id_token",     // This code may crash if you change ResponseType
                    Scope = bankIdSettings.Scope.ToString(),
                    SignInAsAuthenticationType = "Cookies",

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        RedirectToIdentityProvider = context =>
                        {
                            var logger = log4net.LogManager.GetLogger("File");
                            logger.Info("RedirectToIdentityProvider");
                            // Note! OWIN uses response_mode == "form_post" - not the default fragment!
                            if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.AuthenticationRequest)
                            {
                                // Add custom parameters for BID OIDC here:
                                string login_hint = context.OwinContext.Get<string>(OpenIdConnectParameterNames.LoginHint);
                                login_hint = "BID:02119206106";
                                if (!string.IsNullOrEmpty(login_hint))
                                {
                                    // If going thru Azure AD/B2C we need to use custom login_hint:
                                    // BankID OIDC server treats bid_login_hint as login_hint
                                    context.ProtocolMessage.Parameters.Add(OpenIdConnectParameterNames.LoginHint, login_hint);
                                }
                                string ui_locales = context.OwinContext.Get<string>(OpenIdConnectParameterNames.UiLocales);
                                if (!string.IsNullOrEmpty(ui_locales))
                                {
                                    context.ProtocolMessage.Parameters.Add(OpenIdConnectParameterNames.UiLocales, ui_locales);
                                }
                            }
                            return Task.FromResult(0);
                        },

                        AuthorizationCodeReceived = async (context) =>
                        {
                            var logger = log4net.LogManager.GetLogger("File");
                            logger.Info("AuthorizationCodeReceived");
                            // Here we have an authorization_code and needs to get the access_token. 
                            await GetAccessTokenAndStoreWithIdentity(context);
                        },

                        AuthenticationFailed = context =>
                        {
                            var logger = log4net.LogManager.GetLogger("File");
                            logger.Info("AuthenticationFailed");
                            context.Response.Redirect(bankIdSettings.RedirectUrl.ToString() + "Home/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        },

                        SecurityTokenValidated = context =>
                        {
                            var logger = log4net.LogManager.GetLogger("File");
                            logger.Info("SecurityTokenValidated");

                            var ident = context.AuthenticationTicket.Identity;

                            // OWIN Middleware translates claim types from BID's id_token:
                            // "sub" becomes "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                            // "dateofbirth" becomes "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth"
                            // Make a copy of the BankID unique ID to be on the safe side (don't trust AAD to keep off)
                            Claim bid = ident.FindFirst(System.IdentityModel.Claims.ClaimTypes.NameIdentifier);
                            if (bid != null)
                            {
                                logger.Info("    bid = " + JsonConvert.SerializeObject(new {
                                    Id = "bid_id",
                                    Value = bid.Value,
                                    ValueType = bid.ValueType,
                                    Issuer = bid.Issuer,
                                    OriginalIssuer = bid.OriginalIssuer,
                                    Subject = new {
                                        Name = bid.Subject.Name,
                                        Label = bid.Subject.Label,
                                        Actor = bid.Subject.Actor,
                                        IsAuthenticated = bid.Subject.IsAuthenticated,
                                        AuthenticationType = bid.Subject.AuthenticationType
                                    }
                                }));
                                context.AuthenticationTicket.Identity.AddClaim(
                                    new Claim(
                                        "bid_id",
                                        bid.Value,
                                        bid.ValueType,
                                        bid.Issuer,
                                        bid.OriginalIssuer,
                                        bid.Subject
                                    ));

                                context.AuthenticationTicket.Identity.AddClaim(
                                    new Claim(
                                        "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", 
                                        bid.Value
                                        ));
                            }

                            // Ensure that this identity has got a name
                            Claim userName = ident.FindFirst("preferred_username") ?? ident.FindFirst("name");
                            if (userName != null)
                            {
                                logger.Info("    Preferred username = " + userName);
                                context.AuthenticationTicket.Identity.AddClaim(new Claim(ident.NameClaimType, userName.Value));
                            }

                            return Task.FromResult(0);
                        }
                    }
                };

                app.UseOpenIdConnectAuthentication(options);
            }
        }

        //
        // Call the OAuth2 token endpoint to get an access_token and accepted scopes and add to identity's claims.
        //
        private async Task GetAccessTokenAndStoreWithIdentity(AuthorizationCodeReceivedNotification context)
        {
            var logger = log4net.LogManager.GetLogger("File");
            logger.Info("GetAccessTokenAndStoreWithIdentity");

            string access_token = string.Empty;
            string allowed_scopes = string.Empty;

            // Save id_token for display in HomeController 
            // (id_token is allready used to build this identity, but we want the unaltered version).
            string id_token = context.JwtSecurityToken.ToString();
            context.AuthenticationTicket.Identity.AddClaim(new Claim(OpenIdConnectParameterNames.IdToken, id_token));

            using (var client = new HttpClient())
            {
                var configuration = await context.Options.ConfigurationManager.GetConfigurationAsync(context.Request.CallCancelled);

                var request = new HttpRequestMessage(HttpMethod.Post, configuration.TokenEndpoint);
                Dictionary<string, string> reqDictionary = new Dictionary<string, string>
                {
                    { OpenIdConnectParameterNames.ClientId, context.Options.ClientId },
                    { OpenIdConnectParameterNames.ClientSecret, context.Options.ClientSecret },
                    { OpenIdConnectParameterNames.Code, context.ProtocolMessage.Code },
                    { OpenIdConnectParameterNames.GrantType, "authorization_code" },
                    { OpenIdConnectParameterNames.RedirectUri, context.Options.RedirectUri }
                };


                request.Content = new FormUrlEncodedContent(reqDictionary);

                HttpResponseMessage response = null;
                try
                {
                    response = await client.SendAsync(request, context.Request.CallCancelled);
                    response.EnsureSuccessStatusCode();
                    logger.Info("  Response received");
                }
                catch (Exception e)
                {
                    logger.Info("  Exception - " + e.Message);

                    string message = e.Message;
                    var path = HttpContext.Current.Server.MapPath("Home/Error?message=" + HttpUtility.UrlPathEncode(message));
                    context.Response.Redirect(path);
                    return;
                }
                var jsonPayload = await response.Content.ReadAsStringAsync();
                logger.Debug("    " + jsonPayload);

                var payload = JObject.Parse(jsonPayload);
                access_token = payload.Value<string>(OpenIdConnectParameterNames.AccessToken) ?? string.Empty;
                allowed_scopes = payload.Value<string>(OpenIdConnectParameterNames.Scope) ?? string.Empty;

            }

            logger.Info("  Reading tokens");
            context.AuthenticationTicket.Identity.AddClaim(new Claim(OpenIdConnectParameterNames.AccessToken, access_token));
            context.AuthenticationTicket.Identity.AddClaim(new Claim("allowed_scopes", allowed_scopes));
        }
    }
}
