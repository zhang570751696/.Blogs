using CNBlogs.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBlogs.Frame
{
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<User> Users { get; set; }

        protected string ConnectionStr { get; set; }

        public BlogContext(string connectionString)
        {
            this.ConnectionStr = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql(this.ConnectionStr);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var etUser = modelBuilder.Entity<User>();
            etUser.ToTable("BlogUser");

            var etBlog = modelBuilder.Entity<Blog>();
            etBlog.ToTable("UserBlog");

            etBlog.HasOne(b => b.User).WithMany().HasForeignKey(u => u.UserId).IsRequired();
        }
    }
}
