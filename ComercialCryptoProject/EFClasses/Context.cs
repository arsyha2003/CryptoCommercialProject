using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ArbiBot
{
    public class Context : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<TypesOfSubscribe> Types { get; set; }
        public DbSet<BybitPareInfo> BybitInfo { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CryptoProject;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BybitPareInfo>().Property(i => i.AvgPrice).HasColumnType("decimal(28)");
            modelBuilder.Entity<UserData>()
                .HasOne(u => u.SubType)
                .WithMany()
                .HasForeignKey(u => u.SubTypeId);
        }

    }
}
