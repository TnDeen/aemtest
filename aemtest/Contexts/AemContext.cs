using aemtest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Contexts
{
    public class AemContext : DbContext
    {
        public AemContext(DbContextOptions<AemContext> options) : base(options)
        {
        }

        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Well> Wells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>().HasIndex(a => a.SyncId).IsUnique();
            modelBuilder.Entity<Well>().HasIndex(a => a.SyncId).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
