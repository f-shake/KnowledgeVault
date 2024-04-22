using KnowledgeVault.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeVault.Core
{
    public class KnowledgeVaultDbContext : DbContext
    {

        private static readonly string connectionString = "Data Source=KnowledgeVault.db";

        public KnowledgeVaultDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<AchievementEntity> Achievements { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
