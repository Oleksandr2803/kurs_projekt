using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Kurs_Proj.test
{
    public class ApplicationContext : DbContext
    {
        private string cs = "Host=localhost;Port=5432;Database=Loh;Username=root;Password=123456789";
        public DbSet<Comands> Comand { get; set; }
        public DbSet<Loh> Loh { get; set; }
        public DbSet<Networks> Networks{ get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(cs); // Assuming you're using Npgsql provider for PostgreSQL
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comands>()
                .HasMany(e => e.Lohs)
                .WithOne(e => e.Comand)
                .HasForeignKey(e => e.ComandID)
                .IsRequired(false);

            modelBuilder.Entity<Networks>()
                .HasMany(b => b.Lohs)
                .WithOne(b => b.Network)
                .HasForeignKey(b => b.NetworkID)
                .IsRequired(false);
        }



    }
}
