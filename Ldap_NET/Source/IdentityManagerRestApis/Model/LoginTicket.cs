using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityManagerRestApis.Model
{
    public class LoginTicket
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public Guid TokenId { get; set; }
        public DateTime LogInTime { get; set; }
        public bool IsExpired { get; set; }
    }
}