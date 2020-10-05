using Microsoft.EntityFrameworkCore;
using BBKBootcampSocial.Domains;

namespace BBKBootcampSocial.DataLayer
{
    public class BBKDatabaseContext : DbContext
    {
        #region Constructor
        public BBKDatabaseContext(DbContextOptions<BBKDatabaseContext> options) : base(options) { }
        #endregion


        #region Db Sets

        public DbSet<User> Users { get; set; }
        #endregion

    }
}
