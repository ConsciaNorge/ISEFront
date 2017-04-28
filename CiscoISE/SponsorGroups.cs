using CiscoISE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE
{
    public class SponsorGroups
    {
        public static async Task<List<SponsorGroupBriefViewModel>> Get(ISEConnection connection)
        {
            var response = await connection.RestGet("config/sponsorgroup");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    SearchResult = new
                    {
                        total = (int)0,
                        resources = new List<SponsorGroupBriefViewModel>()
                    }
                };

                var sponsorGroups = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    sponsorGroups != null &&
                    sponsorGroups.SearchResult != null &&
                    sponsorGroups.SearchResult.resources != null) ?
                    sponsorGroups.SearchResult.resources :
                    null;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public static async Task<SponsorGroupViewModel> Get(ISEConnection connection, SponsorGroupBriefViewModel portal)
        {
            return await Get(connection, portal.Id.Value);
        }

        public static async Task<SponsorGroupViewModel> Get(ISEConnection connection, Guid id)
        {
            var response = await connection.RestGet("config/sponsorgroup/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var typeDef = new
                {
                    SponsorGroup = new SponsorGroupViewModel()
                };

                var getResult = JsonConvert.DeserializeAnonymousType(json, typeDef);

                return (
                    getResult != null &&
                    getResult.SponsorGroup != null) ?
                    getResult.SponsorGroup :
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
