using System;
using CiscoISE.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CiscoISE.Tests
{
    [TestClass]
    public class GuestUsers
    {
        public Uri baseUrl = new Uri("https://hvciscoise.munchkinlan.com:9060");
        public string username = "sponsoruser";
        public string password = "Minions12345";
        public string guestPortal = "4c3ca030-e371-11e6-92ce-005056873bd0";

        [TestMethod]
        public async Task GetAllUsers()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var users = await CiscoISE.GuestUsers.Get(connection);

            Assert.IsNotNull(users);
        }

        [TestMethod]
        public async Task GetUser()
        {
            var connection = new ISEConnection(baseUrl, username, password);
            var user = await CiscoISE.GuestUsers.Get(connection, "bminion");

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task SuspendUser()
        {
            var connection = new ISEConnection(baseUrl, username, password);

            var deleteOk = await DeleteUserIfExist(connection, "dummyuser");
            Assert.IsTrue(deleteOk, "User already exists and cannot be delete");

            var createOk = await CreateValidNewUser(connection, "dummyuser");
            Assert.IsTrue(createOk, "Failed to create user");

            Trace.WriteLine("Getting record of new user");
            var user = await CiscoISE.GuestUsers.Get(connection, "dummyuser");
            Assert.IsNotNull(user, "Can't retreive user following creation");

            Trace.WriteLine("Suspending user");
            var suspendOk = await CiscoISE.GuestUsers.Suspend(connection, user, "Dog ate my homework");

            Trace.WriteLine("Deleting user");
            var deleteTestOk = await CiscoISE.GuestUsers.Delete(connection, user);
            Assert.IsTrue(deleteTestOk, "Cannot delete user");

            Assert.IsTrue(suspendOk, "Failed to make valid REST call for suspending user");
        }

        [TestMethod]
        public async Task CreateandDeleteUser()
        {
            var connection = new ISEConnection(baseUrl, username, password);

            var deleteOk = await DeleteUserIfExist(connection, "dummyuser");
            Assert.IsTrue(deleteOk, "User already exists and cannot be delete");

            var createOk = await CreateValidNewUser(connection, "dummyuser");
            Assert.IsTrue(createOk, "Failed to create user");

            Trace.WriteLine("Getting record of new user");
            var user = await CiscoISE.GuestUsers.Get(connection, "dummyuser");
            Assert.IsNotNull(user, "Can't retreive user following creation");

            Trace.WriteLine("Deleting user");
            var deleteTestOk = await CiscoISE.GuestUsers.Delete(connection, user);
            Assert.IsTrue(deleteTestOk, "Cannot delete user");
        }

        private async Task<bool> DeleteUserIfExist(ISEConnection connection, string username)
        {
            Trace.WriteLine("Retrieving user from ISE if it already exist");
            var user = await CiscoISE.GuestUsers.Get(connection, username);

            if (user != null)
            {
                Trace.WriteLine("Deleting user since it is hanging from earlier test");
                return await CiscoISE.GuestUsers.Delete(connection, user);
            }

            return true;
        }

        private async Task<bool> CreateValidNewUser(ISEConnection connection, string username)
        {
            Trace.WriteLine("Creating new user");
            return await CiscoISE.GuestUsers.Create(
                connection,
                new GuestUserViewModel
                {
                    GuestType = "Contractor (default)",
                    PortalId = guestPortal,
                    GuestInfo = new GuestInfoViewModel
                    {
                        Username = username,
                        Password = "Minions12345",
                        FirstName = "dummy",
                        LastName = "user",
                        EmailAddress = "dummyuser@funkychicken.org",
                        Enabled = true
                    },
                    GuestAccessInfo = new GuestAccessInfoViewModel
                    {
                        ValidDays = 100,
                        Location = "San Jose"
                    }
                });
        }
    }
}
