using IdentityManagerRestApis.Model;

namespace IdentityManagerRestApis.Repositories.Interfaces
{
    public interface ILoginTicketRepository
    {
        LoginTicket GetLoginTicket(string userNameWithDomain);
        void CreateLoginTicket(string userNameWithDomain, string ldapPath);
        void ExpireLoginTicket(LoginTicket loginTicket);
       
    }
}