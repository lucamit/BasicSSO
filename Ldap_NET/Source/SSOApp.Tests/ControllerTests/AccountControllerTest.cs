using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SSOApp.Controllers;
using SSOApp.Models;

namespace SSOApp.Tests.ControllerTests
{
     [TestFixture]
   public class AccountControllerTest
    {
        private Mock<AccountController> accountController;

        public void TestLogin()
        {
            accountController.Setup(x => x.ApiCall(It.IsAny<string>()))
               .Returns(new Tuple<HttpStatusCode, string>(HttpStatusCode.OK, ""));
            var controller = accountController.Object;
            var loginModel = new LoginModel
            {
                UserName = "zflexsoftwares//guest1",
                Password = "guest1password"
            };
            var result = controller.Login(loginModel, "http://localhost/Home?Index") as ViewResult ;
            Assert.AreEqual(result.ViewBag.ReturnUrl, "http://localhost/Home?Index");
        }

        [SetUp]
        public void Setup()
        {
            accountController = new Mock<AccountController>(); 
        }


        [TearDown]
        public void CleanUp()
        {
            accountController = null;
        }
    }
}
