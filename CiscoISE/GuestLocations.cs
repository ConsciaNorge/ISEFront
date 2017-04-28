using CiscoISE.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CiscoISE
{
    public class GuestLocations
    {
        public static async Task<List<GuestLocationViewModel>> Get(ISEConnection connection)
        {
            var response = await connection.RestGet("config/guestlocation");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    SearchResult = new
                    {
                        total = (int)0,
                        resources = new List<GuestLocationViewModel>()
                    }
                };

                var portals = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    portals != null &&
                    portals.SearchResult != null &&
                    portals.SearchResult.resources != null) ?
                    portals.SearchResult.resources :
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
