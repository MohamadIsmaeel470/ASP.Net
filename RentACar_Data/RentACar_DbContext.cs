using Microsoft.EntityFrameworkCore;
using RentACar_Domin;

namespace RentACar_Data
{
    public class RentACar_DbContext : DbContext
    {
        public RentACar_DbContext(DbContextOptions<RentACar_DbContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branch { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehicleType> VehicleType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                 .HasOne(e => e.Branch)
                 .WithMany(b => b.Employees)
                 .HasForeignKey(e => e.BranchID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(b => b.Branch)
                .WithMany(v => v.Vehicles)
                .HasForeignKey(b => b.BranchID);

            modelBuilder.Entity<Vehicle>()
               .HasOne(vt => vt.VehicleType)
               .WithMany(v => v.Vehicles)
               .HasForeignKey(vt => vt.VehicleTypeID);

            modelBuilder.Entity<Payment>()
                .HasOne(r => r.Rental)
                .WithMany(p => p.Payments)
                .HasForeignKey(r => r.RentalID);

            modelBuilder.Entity<Rental>()
               .HasOne(r => r.Customer)
               .WithMany(c => c.Rentals)
               .HasForeignKey(cr => cr.CustomerID)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
