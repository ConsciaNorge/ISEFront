using CiscoISE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE
{
    public class Portals
    {
        public static async Task<List<PortalBriefViewModel>> Get(ISEConnection connection)
        {
            var response = await connection.RestGet("config/portal");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    SearchResult = new
                    {
                        total = (int)0,
                        resources = new List<PortalBriefViewModel>()
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

        public static async Task<PortalViewModel> Get(ISEConnection connection, PortalBriefViewModel portal)
        {
            return await Get(connection, portal.Id.Value);
        }

        public static async Task<PortalViewModel> Get(ISEConnection connection, Guid id)
        {
            var response = await connection.RestGet("config/portal/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    ERSPortal = new PortalViewModel()
                };

                var getResult = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    getResult != null &&
                    getResult.ERSPortal != null) ?
                    getResult.ERSPortal :
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
