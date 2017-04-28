using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace CiscoISE.Tests
{
    [TestClass]
    public class GuetLocations
    {
        public Uri baseUrl = new Uri("https://hvciscoise.munchkinlan.com:9060");
        public string username = "developer";
        public string password = "Minions12345";

        [TestMethod]
        public async Task GetAll()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var locations = await CiscoISE.GuestLocations.Get(connection);

            Assert.IsNotNull(locations);
        }
    }
}
