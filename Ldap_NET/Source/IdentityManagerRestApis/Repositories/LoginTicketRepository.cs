using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using IdentityManagerRestApis.Context;
using IdentityManagerRestApis.Model;
using IdentityManagerRestApis.Repositories.Interfaces;

namespace IdentityManagerRestApis.Repositories
{
    public class LoginTicketRepository : ILoginTicketRepository
    {
        private  static object _lockObject = new object();
        public LoginTicket GetLoginTicket(string userName)
        {
            using (var ctx = new IdentityManagerContext())
            {
                var ticket =
                    ctx.Tickets.Where(
                        x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && x.IsExpired == false)
                        .OrderByDescending(x => x.LogInTime).FirstOrDefault();

                return ticket;
            }
        }

        public void CreateLoginTicket(string userNameWithDomain, string ldapPath)
        {
            var ticket = new LoginTicket
            {
                Domain = ldapPath,
                IsExpired = false,
                LogInTime = DateTime.UtcNow,
                TokenId = Guid.NewGuid(),
                UserName = userNameWithDomain,
            };
            using (var ctx = new IdentityManagerContext())
            {
                ctx.Tickets.Add(ticket);
                ctx.SaveChanges();
            }
        }

        public void ExpireLoginTicket(LoginTicket loginTicket)
        {
            using (var ctx = new IdentityManagerContext())
            {
                var ticket = ctx.Tickets.SingleOrDefault(x => x.TokenId == loginTicket.TokenId);
                ticket.IsExpired = true;
                ctx.SaveChanges();
            }
        }


        public static void ExpireLoginTicketAfterTimeout()
        {
            int defaultTimeout = 20;
            int.TryParse(ConfigurationManager.AppSettings["TicketExpirationTimeoutInMinutes"], out defaultTimeout);

            lock (_lockObject)
            {
                using (var ctx = new IdentityManagerContext())
                {
                    var currentTime = DateTime.UtcNow;
                    var tickets = ctx.Tickets.Where(x => x.IsExpired == false && DbFunctions.DiffMinutes(x.LogInTime, currentTime) >= defaultTimeout);
                    foreach (var ticket in tickets)
                    {
                        ticket.IsExpired = true;
                    }
                    ctx.SaveChanges();
                } 
            }
        }
    }
}