using System.Data.Entity;
using eUseControl.Domain.Entities.Profile;

namespace eUseControl.BusinessLogic.DBModel
{
    public class ProfileContext : DbContext
    {
        public ProfileContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<ProfileDbTable> UserProfiles { get; set; }
    }
}
