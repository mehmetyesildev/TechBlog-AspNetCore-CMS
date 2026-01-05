using BlogApp.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext:IdentityDbContext<User,AppRole,int>
    {
        public BlogContext(DbContextOptions<BlogContext>options):base(options)
        {
            
        }
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();
        //public DbSet<User> Users => Set<User>();
    }
}