using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Data
{
    public class Connection : DbContext
    {
        public DbSet<Role> Role { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<QualityControl> QualityControl { get; set; }
        public DbSet<PickupRequest> PickupRequest {  get; set; }






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=postgres;Password=111;Database=SuperPharmacy;");
        }
    }
}
