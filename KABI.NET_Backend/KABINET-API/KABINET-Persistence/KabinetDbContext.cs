using KABINET_Application.ViewModels.Laundry;
using KABINET_Application.ViewModels.TavernAppointment;
using KABINET_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace KABINET_Persistance
{
    public class KabinetDbContext : DbContext
    {
        public KabinetDbContext(DbContextOptions<KabinetDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Laundry> Laundries { get; set; }
        public DbSet<TavernAppointment> TavernAppointments { get; set; }
        public DbSet<LaundryFullReportVm> LaundryFullReports { get; set; }
        public DbSet<TavernAppointmentFullReportVm> TavernAppointmentFullReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Laundries)
                .WithOne(l => l.User);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TavernAppointments)
                .WithOne(ta => ta.User);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValue<Guid>();

            var userLaundryNavigation = modelBuilder.Entity<User>().Metadata.FindNavigation(nameof(User.Laundries));
            userLaundryNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var userTaverbAppointmentNavigation = modelBuilder.Entity<User>().Metadata.FindNavigation(nameof(User.TavernAppointments));
            userTaverbAppointmentNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Laundry>()
               .ToTable("Laundries")
               .HasKey(l => l.Id);

            modelBuilder.Entity<Laundry>()
                .Property(u => u.Id)
                .HasDefaultValue<Guid>();

            modelBuilder.Entity<TavernAppointment>()
               .ToTable("TavernAppointments")
               .HasKey(ta => ta.Id);

            modelBuilder.Entity<TavernAppointment>()
                .Property(u => u.Id)
                .HasDefaultValue<Guid>();

            modelBuilder.Entity<LaundryFullReportVm>()
                .ToView("LaundryFullReport")
                .HasNoKey();

            modelBuilder.Entity<TavernAppointmentFullReportVm>()
               .ToView("TavernAppointmentFullReport")
               .HasNoKey();

            // Seeding data
            modelBuilder.Seed();
        }
    }
}
