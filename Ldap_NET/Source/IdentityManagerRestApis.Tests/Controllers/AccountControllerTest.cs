using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdentityManagerRestApis.Controllers;

namespace IdentityManagerRestApis.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void GetForValidLogin()
        {
            var controller = new AccountController();

            // Act
            var result = controller.Get("zflex","guest1","guest1password","ldap://zflexldap.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);;
        }

        [TestMethod]
        public void GetForInValidLogin()
        {
            var controller = new AccountController();

            // Act
            var result = controller.Get("zflex", "guest1", "guest", "ldap://zflexldap.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized); ;
        }
 

    }
}
