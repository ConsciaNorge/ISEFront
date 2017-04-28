using ISEFront.Utility.CiscoISE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace libciscoise
{
    public class GuestUsers
    {
        public static async Task<List<GuestUserBriefViewModel>> Get(ISEConnection connection)
        {
            var response = await connection.RestGet("config/guestuser");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    SearchResult = new
                    {
                        total = (int)0,
                        resources = new List<GuestUserBriefViewModel>()
                    }
                };

                var guestUsers = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    guestUsers != null &&
                    guestUsers.SearchResult != null &&
                    guestUsers.SearchResult.resources != null) ?
                    guestUsers.SearchResult.resources :
                    null;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public static async Task<GuestUserViewModel> Get(ISEConnection connection, string name)
        {
            var response = await connection.RestGet("config/guestuser/name/" + name);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new { GuestUser = new GuestUserViewModel() };
                var guestUser = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (guestUser != null && guestUser.GuestUser != null) ?
                    guestUser.GuestUser :
                    null;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public static async Task<GuestUserViewModel> Get(ISEConnection connection, GuestUserBriefViewModel brief)
        {
            return await Get(connection, brief.Id);
        }

        public static async Task<GuestUserViewModel> Get(ISEConnection connection, Guid id)
        {
            var response = await connection.RestGet("config/guestuser/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new { GuestUser = new GuestUserViewModel() };
                var guestUser = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (guestUser != null && guestUser.GuestUser != null) ?
                    guestUser.GuestUser :
                    null;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public static async Task<bool> Sms(ISEConnection connection, GuestUserBriefViewModel user)
        {
            return await Sms(connection, user.Id);
        }

        public static async Task<bool> Sms(ISEConnection connection, Guid id)
        {
            return await connection.RestPut(
                "config/guestuser/sms/" + id.ToString(), 
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Reinstate(ISEConnection connection, GuestUserBriefViewModel user)
        {
            return await Reinstate(connection, user.Id);
        }

        public static async Task<bool> Reinstate(ISEConnection connection, Guid id)
        {

            return await connection.RestPut(
                "config/guestuser/reinstate/" + id.ToString(), 
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Suspend(ISEConnection connection, GuestUserBriefViewModel user, string reason)
        {
            return await Suspend(connection, user.Id, reason);
        }

        public static async Task<bool> Suspend(ISEConnection connection, Guid id, string reason)
        {
            var additionalData = new
            {
                OperationAdditionalData = new
                {
                    additionalData = new List<Object>
                    {
                        new {
                            name = "reason",
                            value = reason 
                        }
                    }
                }
            };

            return await connection.RestPut(
                "config/guestuser/suspend/" + id.ToString(), 
                HttpStatusCode.NoContent, 
                additionalData);
        }

        public static async Task<bool> Deny(ISEConnection connection, GuestUserBriefViewModel user, string reason)
        {
            return await Deny(connection, user.Id, reason);
        }

        public static async Task<bool> Deny(ISEConnection connection, Guid id, string reason)
        {
            return await connection.RestPut(
                "config/guestusers/deny/" + id.ToString(), 
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Approve(ISEConnection connection, GuestUserBriefViewModel user, string reason)
        {
            return await Approve(connection, user.Id, reason);
        }

        public static async Task<bool> Approve(ISEConnection connection, Guid id, string reason)
        {
            return await connection.RestPut(
                "config/guestusers/approve/" + id.ToString(),
                HttpStatusCode.NoContent);
        }
    }
}