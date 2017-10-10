using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using IdentityManagerRestApis.Model;

namespace IdentityManagerRestApis.Context
{
    public class IdentityManagerContext :DbContext
    {
        public IdentityManagerContext()
            : base("name=IdentityManager")
        {

        }
        
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    
        public virtual DbSet<LoginTicket> Tickets { get; set; }
    }
}