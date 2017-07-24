using CiscoISE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CiscoISE
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
            HttpResponseMessage response;
            try
            {
                response = await connection.RestGet("config/guestuser/name/" + name);
            } catch {
                System.Diagnostics.Trace.WriteLine("Failed to get result from config/guestuser/name/" + name, "ISEError");
                return null;
            }

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
            return await Get(connection, brief.Id.Value);
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
            return await Sms(connection, user.Id.Value);
        }

        public static async Task<bool> Sms(ISEConnection connection, Guid id)
        {
            return await connection.RestPut(
                "config/guestuser/sms/" + id.ToString(), 
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Reinstate(ISEConnection connection, GuestUserBriefViewModel user)
        {
            return await Reinstate(connection, user.Id.Value);
        }

        public static async Task<bool> Reinstate(ISEConnection connection, Guid id)
        {

            return await connection.RestPut(
                "config/guestuser/reinstate/" + id.ToString(), 
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Suspend(ISEConnection connection, GuestUserBriefViewModel user, string reason)
        {
            return await Suspend(connection, user.Id.Value, reason);
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
            return await Deny(connection, user.Id.Value, reason);
        }

        public static async Task<bool> Deny(ISEConnection connection, Guid id, string reason)
        {
            return await connection.RestPut(
                "config/guestusers/deny/" + id.ToString(), 
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Approve(ISEConnection connection, GuestUserBriefViewModel user, string reason)
        {
            return await Approve(connection, user.Id.Value, reason);
        }

        public static async Task<bool> Approve(ISEConnection connection, Guid id, string reason)
        {
            return await connection.RestPut(
                "config/guestusers/approve/" + id.ToString(),
                HttpStatusCode.NoContent);
        }

        public static async Task<bool> Create(ISEConnection connection, GuestUserViewModel user)
        {
            if (user.GuestAccessInfo == null)
                throw new ArgumentNullException("GuestAccessInfo");
            if(user.GuestInfo == null)
                throw new ArgumentNullException("GuestInfo");

            if (user.GuestAccessInfo.ValidDays < 1)
                throw new IndexOutOfRangeException("ValidDays must be 1 or greater");

            if (!user.GuestAccessInfo.FromDate.HasValue && user.GuestAccessInfo.ToDate.HasValue)
                throw new ArgumentException("fromDate not set, but toDate is", "FromDate");

            if (!user.GuestAccessInfo.FromDate.HasValue)
                user.GuestAccessInfo.FromDate = DateTime.Now;

            // TODO - Figure out how to make 1 day happen
            if (!user.GuestAccessInfo.ToDate.HasValue)
                user.GuestAccessInfo.ToDate = user.GuestAccessInfo.FromDate.Value.AddDays(user.GuestAccessInfo.ValidDays - 1);

            // Adjust local time to server time (currently, we assume UTC)
            // TODO : Fetch time zone information from the ISE server
            user.GuestAccessInfo.FromDate = user.GuestAccessInfo.FromDate.Value.ToUniversalTime();
            user.GuestAccessInfo.ToDate = user.GuestAccessInfo.ToDate.Value.ToUniversalTime();

            return await connection.RestPost(
                "config/guestuser",
                HttpStatusCode.Created,
                new
                {
                    GuestUser = user
                }
                );
        }

        public static async Task<bool> Delete(ISEConnection connection, GuestUserBriefViewModel user)
        {
            return await Delete(connection, user.Id.Value);
        }

        public static async Task<bool> Delete(ISEConnection connection, Guid id)
        {
            return await connection.RestDelete(
                "config/guestuser/" + id.ToString(),
                HttpStatusCode.NoContent);
        }
    }
}