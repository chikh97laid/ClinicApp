using AutoMapper;
using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Clinic_Project.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IPersonRepo Persons { get; }
        public IPatientRepo Patients { get; }
        public IDoctorRepo Doctors { get; }
        public IAppointmentRepo Appointments { get; }
        public IPrescriptionRepo Prescriptions { get; }
        public IMedicationRepo Medications {  get; }
        public IRecordRepo Records { get; }
        public IPaymentRepo Payments { get; }
        public IPaymentMethodRepo PaymentMethods { get; }
        public ISpecializationRepo Specializations { get; }
        public IPrescriptionMedicationRepo PrescriptionMedications { get; }
        public IRefreshTokenRepo RefreshTokens { get; }

        public UnitOfWork(AppDbContext context, IPersonRepo persons, IPatientRepo patients, IDoctorRepo doctors,
               IAppointmentRepo appointments, IPrescriptionRepo prescriptions, IMedicationRepo medications,
               IRecordRepo records, IPaymentRepo payments, IPaymentMethodRepo paymentMethods,
               ISpecializationRepo specializations, IPrescriptionMedicationRepo prescriptionMedications,
               IRefreshTokenRepo refreshTokens)
        {
            _context = context;
            Persons = persons;
            Patients = patients;
            Doctors = doctors;
            Appointments = appointments;
            Prescriptions = prescriptions;
            Medications = medications;
            Records = records;
            Payments = payments;
            PaymentMethods = paymentMethods;
            Specializations = specializations;
            PrescriptionMedications = prescriptionMedications;
            RefreshTokens = refreshTokens;
        }

        public async Task CommitChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
