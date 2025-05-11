using Microsoft.EntityFrameworkCore;
using MallManagmentSystem.Models;
namespace MallManagmentSystem.Data
{
    public class MallDBContext : DbContext
    {
        public MallDBContext(DbContextOptions<MallDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precisions
            modelBuilder.Entity<DebitForNonRenter>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DebitForRenter>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<EmployeeLoan>()
                .Property(l => l.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<EmploymentContract>()
                .Property(c => c.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Mall>()
                .Property(m => m.TotalExpenses)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Mall>()
                .Property(m => m.TotalRevenue)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Store>()
                .Property(s => s.MonthlyRentAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StorePenalty>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StoreRentContract>()
                .Property(c => c.DepositAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StoreRentContract>()
                .Property(c => c.MonthlyRent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StoreRentContract>()
                .Property(c => c.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WorkPenaltyDeduction>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Mall> Malls { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmploymentContract> EmploymentContracts { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<StoreRentContract> StoreRentContracts { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public DbSet<RentInvoice> RentInvoices { get; set; }
        public DbSet<WorkPenaltyDeduction> WorkPenaltyDeductions { get; set; }
        public DbSet<StorePenalty> StorePenalties { get; set; }
        public DbSet<DebitForRenter> DebitForRenters { get; set; }
        public DbSet<DebitForNonRenter> DebitForNonRenters { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
