using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace CiscoISE.Tests
{
    [TestClass]
    public class Portals
    {
        public Uri baseUrl = new Uri("https://hvciscoise.munchkinlan.com:9060");
        public string username = "developer";
        public string password = "Minions12345";

        [TestMethod]
        public async Task GetAll()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var portals = await CiscoISE.Portals.Get(connection);

            Assert.IsNotNull(portals);
        }

        [TestMethod]
        public async Task GetFirst()
        {
            var connection = new ISEConnection(baseUrl, username, password);

            var portals = await CiscoISE.Portals.Get(connection);
            Assert.IsNotNull(portals);

            var portal = await CiscoISE.Portals.Get(connection, portals.First());
            Assert.IsNotNull(portal);
        }
    }
}
