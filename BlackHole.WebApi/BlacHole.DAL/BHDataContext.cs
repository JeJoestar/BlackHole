using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.DAL
{
    public class BHDataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Image> Images { get; set; }

        private string _connectionString;

        public BHDataContext()
        {
            _connectionString = "Data Source=localhost;Initial Catalog=BlackDB;Integrated Security=true;";
        }

        public BHDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Mail = "admin@gmail.com",
                Password = "admin123",
                Role = RoleConstants.Admin,
            });
        }
    }
}
