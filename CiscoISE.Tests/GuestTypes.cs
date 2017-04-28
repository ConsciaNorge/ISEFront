using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace CiscoISE.Tests
{
    [TestClass]
    public class GuestTypes
    {
        public Uri baseUrl = new Uri("https://hvciscoise.munchkinlan.com:9060");
        public string username = "developer";
        public string password = "Minions12345";

        [TestMethod]
        public async Task Get()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var guestTypes = await CiscoISE.GuestTypes.Get(connection);

            Assert.IsNotNull(guestTypes);
        }

        [TestMethod]
        public async Task GetFirst()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var guestTypes = await CiscoISE.GuestTypes.Get(connection);

            Assert.IsNotNull(guestTypes);

            var firstGuestType = await CiscoISE.GuestTypes.Get(connection, guestTypes.First());
            Assert.IsNotNull(firstGuestType);
        }
    }
}
