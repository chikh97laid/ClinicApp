using Clinic_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedication> prescriptionMedications { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
                        .HasOne(p => p.Doctor)
                        .WithOne(d => d.Person)
                        .HasForeignKey<Doctor>(d => d.PersonId);

            modelBuilder.Entity<Person>()
                        .HasOne(p => p.Patient)
                        .WithOne(p => p.Person)
                        .HasForeignKey<Patient>(d => d.PersonId);

            modelBuilder.Entity<Person>().HasIndex(p => p.Email).IsUnique();

            modelBuilder.Entity<Patient>().HasIndex(p => p.PersonId).IsUnique();

            modelBuilder.Entity<Doctor>().HasIndex(d => d.PersonId).IsUnique();

            modelBuilder.Entity<Appointment>().HasIndex(d => d.PatientId);
            modelBuilder.Entity<Appointment>().HasIndex(d => d.DoctorId);
            modelBuilder.Entity<Appointment>().HasIndex(d => d.CreatedAt).IsUnique();

            modelBuilder.Entity<Payment>().HasIndex(d => d.AppointmentId).IsUnique();


            modelBuilder.Entity<Prescription>().HasIndex(d => d.PatientId);
            modelBuilder.Entity<Prescription>().HasIndex(d => d.DoctorId);

            modelBuilder.Entity<Record>().HasIndex(d => d.PatientId);
            modelBuilder.Entity<Record>().HasIndex(d => d.DoctorId);

            modelBuilder.Entity<PrescriptionMedication>()
                        .HasIndex(pm => new { pm.PrescriptionId, pm.MedicationId })
                        .IsUnique();

            modelBuilder.Entity<Person>()
                .Property(p => p.Gender)
                .HasConversion<string>()
                .HasMaxLength(10);

        }

    }
}
