using System;
using System.Web;
using System.Web.Security;

namespace HttpModules.Interceptors
{
    public class IdentityHttpModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        void BeginRequest(object sender, EventArgs e)
        {
            
            var request = HttpContext.Current.Request;
            var cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                var encryptedTicket = cookie.Value;
                var ticket = FormsAuthentication.Decrypt(encryptedTicket);
                if (ticket.Expiration.ToUniversalTime() < DateTime.Now.ToUniversalTime())
                {
                    HttpContext.Current.Response.StatusCode = 401;
                }
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 401;
            }
        }

        public void Dispose()
        {
           
        }
    }
}