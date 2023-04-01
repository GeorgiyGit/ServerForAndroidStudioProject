
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ShopDbContext : IdentityDbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        public ShopDbContext(DbContextOptions options) : base(options)
        {
		}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().HasOne(u => u.Avatar)
                                        .WithOne(i => i.User)
                                        .HasForeignKey<User>(u => u.ImageId)
										.IsRequired(false)
                                        .OnDelete(DeleteBehavior.Cascade);
		}
    }
}
