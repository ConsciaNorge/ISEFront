using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace CiscoISE.Tests
{
    [TestClass]
    public class SponsorGroups
    {
        public Uri baseUrl = new Uri("https://hvciscoise.munchkinlan.com:9060");
        public string username = "developer";
        public string password = "Minions12345";

        [TestMethod]
        public async Task GetAll()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var sponsorGroups = await CiscoISE.SponsorGroups.Get(connection);

            Assert.IsNotNull(sponsorGroups);
        }

        [TestMethod]
        public async Task GetFirst()
        {
            var connection = new ISEConnection(baseUrl, username, password);

            var sponsorGroups = await CiscoISE.SponsorGroups.Get(connection);
            Assert.IsNotNull(sponsorGroups);

            var sponsorGroup = await CiscoISE.SponsorGroups.Get(connection, sponsorGroups.First());
            Assert.IsNotNull(sponsorGroup);
        }
    }
}
