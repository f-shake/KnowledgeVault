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
    public class AppDbContext : DbContext
    {
        private readonly string connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected AppDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<AchievementEntity> Achievements { get; set; }

    }
}
