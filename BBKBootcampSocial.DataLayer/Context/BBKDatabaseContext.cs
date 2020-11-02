using System.Linq;
using BBKBootcampSocial.Domains.Access;
using BBKBootcampSocial.Domains.Post;
using Microsoft.EntityFrameworkCore;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.Domains.Image;
using BBKBootcampSocial.Domains.Comment;
using BBKBootcampSocial.Domains.Canal;

namespace BBKBootcampSocial.DataLayer
{
    public class BBKDatabaseContext : DbContext
    {
        #region Constructor

        public BBKDatabaseContext(DbContextOptions<BBKDatabaseContext> options) : base(options) { }

        #endregion

        #region Db Sets

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Canal> Canals { get; set; }
        public DbSet<CanalUser> CanalUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        #endregion

        #region disable cascading delete in database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Like>().HasQueryFilter(l => !l.IsDelete);

            var cascades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascades)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }
}
