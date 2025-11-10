using Clinic_Project.Dtos.Appointment;
using Clinic_Project.Helpers;
using Clinic_Project.Models;

namespace Clinic_Project.Services.Interfaces
{
    public interface IAppointmentService : IService<AppointmentReadDto, AppointmentWriteDto>
    {
        Task<Result<AppointmentReadDto>?> UpdateAppointmentStatusAsync(int id, enAppointmentStatus status);
        Task<Result<AppointmentConfirmedReadDto>?> ConfirmAppointmentAsync(int id, DateTime scheduledDate);
        Task<Result<AppointmentReadDto>?> CancelAppointmentAsync(int id);
        Task<Result<AppointmentReadDto>?> CompleteAppointmentAsync(int id);
        Task<Result<AppointmentReadDto>?> NoShowAppointmentAsync(int id);
        Task<Result<AppointmentReadDto>?> RescheduleAppointmentAsync(int id, DateTime newDate);
        Task<Result<AppointmentReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin);
        Task<Result<IEnumerable<AppointmentReadDto>>?> GetAppointmentsAsyncByDoctorId(int id);
        Task<Result<IEnumerable<AppointmentReadDto>>?> GetAppointmentsAsyncByPatientId(int id);
        Task<Result<AppointmentReadDto>?> UpdateAsync(int id, AppointmentWriteDto dto, string currentUserId, bool isAdmin);
    }
}
