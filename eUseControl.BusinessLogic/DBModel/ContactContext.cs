using System.Data.Entity;
using eUseControl.Domain.Entities.Contact;

namespace eUseControl.BusinessLogic.DBModel
{
    public class ContactContext : DbContext
    {
        public ContactContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<ContactDbTable> ContactRequests { get; set; }
    }
}
