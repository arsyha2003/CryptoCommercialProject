using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiBot
{
    public class TradeContext : DbContext
    {
        public DbSet<ExchangeInfo> Signals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeInfo>().Property(i=>i.AvgPrice).HasColumnType("money");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SignalsInfo;Trusted_Connection=True;");
        }
    }
}
