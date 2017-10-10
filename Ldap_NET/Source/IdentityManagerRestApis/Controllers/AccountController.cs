using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using IdentityManager.Library.Providers;
using IdentityManagerRestApis.Repositories;
using IdentityManagerRestApis.Repositories.Interfaces;

namespace IdentityManagerRestApis.Controllers
{
    public class AccountController : ApiController
    {
        private static Timer _timer = new Timer((e) => ExpireTokenAfterTimeout(), null, 0, (long)TimeSpan.FromMinutes(1).TotalMilliseconds);
        private readonly ILoginTicketRepository _loginTicketRepository;
        private string _ldapPath;

        public AccountController()
        {
            _ldapPath = ConfigurationManager.AppSettings["Ldap.Connection"];
            _loginTicketRepository = new LoginTicketRepository();
        }

        public HttpResponseMessage Get(string domain, string userName, string password, string ldapPath = "")
        {
            var userNameWithDomain = string.Format(@"{0}\{1}", domain, userName);
            try
            {
                if (!string.IsNullOrWhiteSpace(ldapPath))
                    _ldapPath = ldapPath;

                if (_loginTicketRepository.GetLoginTicket(userNameWithDomain) != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                if (LdapProvider.Authenticate(userNameWithDomain, password, _ldapPath))
                    _loginTicketRepository.CreateLoginTicket(userNameWithDomain, _ldapPath);
                else
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private static void ExpireTokenAfterTimeout()
        {
            LoginTicketRepository.ExpireLoginTicketAfterTimeout();
        }

    }
}