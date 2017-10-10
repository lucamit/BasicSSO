using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SingleSignOnApp.Utilities;
using SSOApp.Models;

namespace SSOApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var domain = string.Empty;
            if (!model.UserName.Contains("\\"))
            {
                ModelState.AddModelError(string.Empty, "Domain is required.Input Format doamin\\username");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            domain = model.UserName.Substring(0, model.UserName.IndexOf('\\'));
            var userNameIWthoutDomian = model.UserName.Substring(model.UserName.LastIndexOf('\\') + 1);
        
            var parameters = string.Format("?domain={0}&userName={1}&password={2}&ldapPath={3}", domain, userNameIWthoutDomian, model.Password, "");
            Tuple<HttpStatusCode, string> response;
            try
            {
                response = ApiCall(parameters);
            }
            catch (Exception)
            {
                
                 ModelState.AddModelError(string.Empty, "Ldap authentication failed.");
                   ModelState.AddModelError(string.Empty, "Invalid login details");
                   ViewBag.ReturnUrl = returnUrl;
                   return View();
            }
            if (response.Item1 == HttpStatusCode.OK)
            {
                var ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now.ToUniversalTime(), DateTime.Now.ToUniversalTime().AddMinutes(20), false, FormsAuthentication.FormsCookiePath);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login details");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        public  Tuple<HttpStatusCode, string> ApiCall(string parameters)
        {
            var apiUrl = ConfigurationManager.AppSettings["AuthenticationApi"];
            var client = new RestClient
            {
                EndPoint = apiUrl,
                Method = HttpVerb.GET,

            };
            return client.MakeRequest(parameters);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}
