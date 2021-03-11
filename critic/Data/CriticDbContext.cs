using Critic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Critic.Data
{
    public class CriticDbContext: DbContext
    {

        public CriticDbContext()
        {

        }

        public CriticDbContext(DbContextOptions<CriticDbContext> options) : base(options)
        {
        }


        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().ToTable("User");
            modelBuilder.Entity<Restaurant>().ToTable("Restaurant");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Reply>().ToTable("Reply");

        }
    }
}
