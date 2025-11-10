
using Clinic_Project.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepo Persons { get; }
        IPatientRepo Patients { get; }
        IDoctorRepo Doctors { get; }
        IAppointmentRepo Appointments { get; }
        IPrescriptionRepo Prescriptions { get; }
        IMedicationRepo Medications { get; }
        IRecordRepo Records { get; }
        IPaymentRepo Payments { get; }
        IPaymentMethodRepo PaymentMethods { get; }
        ISpecializationRepo Specializations { get; }
        IPrescriptionMedicationRepo PrescriptionMedications { get; }
        IRefreshTokenRepo RefreshTokens {  get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitChangesAsync();
    }
}
