using CiscoISE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE
{
    public class GuestTypes
    {
        public static async Task<List<GuestTypeBriefViewModel>> Get(ISEConnection connection)
        {
            var response = await connection.RestGet("config/guesttype");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    SearchResult = new
                    {
                        total = (int)0,
                        resources = new List<GuestTypeBriefViewModel>()
                    }
                };

                var guestTypes = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    guestTypes != null &&
                    guestTypes.SearchResult != null &&
                    guestTypes.SearchResult.resources != null) ?
                    guestTypes.SearchResult.resources :
                    null;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public static async Task<GuestTypeBriefViewModel> Get(ISEConnection connection, GuestTypeBriefViewModel guestType)
        {
            return await Get(connection, guestType.Id.Value);
        }

        public static async Task<GuestTypeBriefViewModel> Get(ISEConnection connection, Guid id)
        {
            var response = await connection.RestGet("config/guesttype/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    GuestType = new GuestTypeViewModel()
                };

                var getResult = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    getResult != null &&
                    getResult.GuestType != null) ?
                    getResult.GuestType :
                    null;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }
    }
}
