using KnowledgeVault.WebAPI.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeVault.WebAPI
{
    public class KnowledgeVaultDbContext : DbContext
    {
        public KnowledgeVaultDbContext(DbContextOptions<KnowledgeVaultDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        private KnowledgeVaultDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<AchievementEntity> Achievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
