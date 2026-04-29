using Microsoft.EntityFrameworkCore;
using NetSentinel.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NetSentinel.Data
{
    public class NetSentinelDbContext : DbContext
    {
        public NetSentinelDbContext(DbContextOptions<NetSentinelDbContext> options) : base(options)
        {
        }

        public DbSet<Website> Websites { get; set; }
        public DbSet<HealthLog> HealthLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Veritabanı tabloları oluşurken özel ayarlar gerekirse buraya yazılır.
            base.OnModelCreating(modelBuilder);
        }
    }
}