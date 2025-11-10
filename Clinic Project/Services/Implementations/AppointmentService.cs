using AutoMapper;
using Clinic_Project.Dtos.Appointment;
using Clinic_Project.Helpers;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Clinic_Project.Services.Interfaces;

namespace Clinic_Project.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AppointmentService(IUnitOfWork uniteOfWork, IMapper mapper)
        {
            _unitOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AppointmentReadDto>?>> GetAllAsync()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            if (appointments == null || !appointments.Any())
            {
                return Result<IEnumerable<AppointmentReadDto>?>.Fail("No Appoinments");
            }

            var readDtoList = _mapper.Map<IEnumerable<AppointmentReadDto>>(appointments);
            return Result<IEnumerable<AppointmentReadDto>?>.Ok(readDtoList);
        }

        public async Task<Result<AppointmentReadDto>?> GetAsyncById(int id, string currentUserId, bool isAdmin)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);
            if (appointment == null) return Result<AppointmentReadDto>.Fail("Appointment not found", enErrorType.NotFound);

            var userId = appointment?.Patient?.Person?.User?.Id;
            var doctorId = appointment?.Doctor?.Person?.User?.Id;
            if (currentUserId != userId && currentUserId != doctorId && !isAdmin)
            {
                return Result<AppointmentReadDto>.Fail("You are not allowed to access this appointment.", enErrorType.Forbiden);
            }

            var appointmentReadDto = _mapper.Map<AppointmentReadDto>(appointment);
            return Result<AppointmentReadDto>.Ok(appointmentReadDto);
        }

        private async Task<ValidationResult> ValidationData(AppointmentWriteDto dto)
        {

            if (!await _unitOfWork.Patients.IsExistAsync(p => p.Id == dto.PatientId))
            {
                return ValidationResult.Fail($"Patient {dto.PatientId} doesn't exist", enErrorType.NotFound);
            }

            if (await _unitOfWork.Doctors.IsExistAsync(p => p.Id == dto.DoctorId))
            {
                return ValidationResult.Fail("Doctor already exists for this appointment", enErrorType.Conflict);
            }

            return ValidationResult.Success();
        }

        public async Task<Result<AppointmentReadDto>?> CreateAsync(AppointmentWriteDto dto)
        {
            var validation = await ValidationData(dto);
            if (!validation.IsValid)
            {
                return Result<AppointmentReadDto>.Fail(validation.Message!, validation.ErrorType);
            }

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = enAppointmentStatus.Pending;

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CommitChangesAsync();

            appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == appointment.Id);

            var appointmentReadDto = _mapper.Map<AppointmentReadDto>(appointment);
            return Result<AppointmentReadDto>.Ok(appointmentReadDto);
        }

        public async Task<Result<AppointmentReadDto>?> UpdateAsync(int id, AppointmentWriteDto dto, string currentUserId, bool isAdmin)
        {
            var validation = await ValidationData(dto);
            if (!validation.IsValid)
            {
                return Result<AppointmentReadDto>.Fail(validation.Message!, validation.ErrorType);
            }

            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);  // means appointment is tracked
            if (appointment == null)
            {
                return Result<AppointmentReadDto>.Fail("Appointment not found");
            }

            var userId = appointment?.Patient?.Person?.User?.Id;
            if (currentUserId != userId && !isAdmin)
            {
                return Result<AppointmentReadDto>.Fail("You are not allowed to access this appointment.", enErrorType.Forbiden);
            }

            _mapper.Map(dto, appointment);

            await _unitOfWork.CommitChangesAsync();

            appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);

            var appointmentReadDto = _mapper.Map<AppointmentReadDto>(appointment);
            return Result<AppointmentReadDto>.Ok(appointmentReadDto);
        }

        public async Task<Result<AppointmentConfirmedReadDto>?> ConfirmAppointmentAsync(int id, DateTime scheduledDate)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);
            if (appointment == null)
            {
                return Result<AppointmentConfirmedReadDto>.Fail("Appointment not found", enErrorType.NotFound);
            }

            if(appointment.Status != enAppointmentStatus.Pending)
            {
                return Result<AppointmentConfirmedReadDto>.Fail("Appointment can only be confirmed if it's pending.", enErrorType.BadRequest);
            }

            if (scheduledDate < DateTime.Now)
                return Result<AppointmentConfirmedReadDto>.Fail("Cannot schedule in the past", enErrorType.BadRequest);

            appointment.Status = enAppointmentStatus.Confirm;
            appointment.ScheduledDate = scheduledDate.ToUniversalTime();

            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentConfirmedReadDto>.Ok(_mapper.Map<AppointmentConfirmedReadDto>(appointment));
        }

        public async Task<Result<AppointmentReadDto>?> UpdateAppointmentStatusAsync(int id, enAppointmentStatus status)
        {
            if (!await _unitOfWork.Appointments.IsExistAsync(ap => ap.Id == id))
            {
                return Result<AppointmentReadDto>.Fail("Appointment not found", enErrorType.NotFound);
            }

            var appointment = new Appointment() { Id = id, Status = status };
            _unitOfWork.Appointments.UpdateAppointmentStatus(appointment);
            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentReadDto>.Ok(new AppointmentReadDto { Status = status});
        }

        public async Task<Result<AppointmentReadDto>?> DeleteAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);
            if (appointment == null) return Result<AppointmentReadDto>.Fail("Appointment not found");

            _unitOfWork.Appointments.Delete(appointment);

            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentReadDto>.Ok(_mapper.Map<AppointmentReadDto>(appointment));
        }

        public async Task<Result<AppointmentReadDto>?> CancelAppointmentAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);

            if (appointment == null)
            {
                return Result<AppointmentReadDto>.Fail("Appointment not found", enErrorType.NotFound);
            }

            if (appointment.Status is enAppointmentStatus.Completed or enAppointmentStatus.Canceled)
                return Result<AppointmentReadDto>.Fail("Appointment cannot be canceled now", enErrorType.BadRequest);


            appointment.Status = enAppointmentStatus.Canceled;
            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentReadDto>.Ok(_mapper.Map<AppointmentReadDto>(appointment));
        }

        public async Task<Result<AppointmentReadDto>?> CompleteAppointmentAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);

            if (appointment == null)
            {
                return Result<AppointmentReadDto>.Fail("Appointment not found", enErrorType.NotFound);
            }

            if (appointment.Status is not enAppointmentStatus.Confirm)
                return Result<AppointmentReadDto>.Fail("Only confirmed appointments can be completed", enErrorType.BadRequest);


            appointment.Status = enAppointmentStatus.Completed;
            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentReadDto>.Ok(_mapper.Map<AppointmentReadDto>(appointment));
        }

        public async Task<Result<AppointmentReadDto>?> NoShowAppointmentAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);

            if (appointment == null)
            {
                return Result<AppointmentReadDto>.Fail("Appointment not found", enErrorType.NotFound);
            }

            if (appointment.Status is not enAppointmentStatus.Confirm)
                return Result<AppointmentReadDto>.Fail("Only confirmed appointments can be marked as NoShow", enErrorType.BadRequest);


            appointment.Status = enAppointmentStatus.NoShow;
            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentReadDto>.Ok(_mapper.Map<AppointmentReadDto>(appointment));
        }

        public async Task<Result<AppointmentReadDto>?> RescheduleAppointmentAsync(int id, DateTime newDate)
        {
            var appointment = await _unitOfWork.Appointments.GetOneAsync(ap => ap.Id == id);

            if (appointment == null)
            {
                return Result<AppointmentReadDto>.Fail("Appointment not found", enErrorType.NotFound);
            }

            if (appointment.Status is not enAppointmentStatus.Confirm)
            {
                return Result<AppointmentReadDto>.Fail("This appointment cannot be rescheduled", enErrorType.BadRequest);
            }


            appointment.Status = enAppointmentStatus.Rescheduled;
            appointment.ScheduledDate = newDate.ToUniversalTime();
            await _unitOfWork.CommitChangesAsync();

            return Result<AppointmentReadDto>.Ok(_mapper.Map<AppointmentReadDto>(appointment));
        }

        public Task<Result<AppointmentReadDto>?> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<AppointmentReadDto>>?> GetAppointmentsAsyncByDoctorId(int DoctorId)
        {
            var appointments = await _unitOfWork.Appointments.FindAsync(x => x.DoctorId == DoctorId);
            if (appointments == null || !appointments.Any())
            {
                return Result<IEnumerable<AppointmentReadDto>?>.Fail("No Appoinments", enErrorType.NotFound);
            }

            var readDtoList = _mapper.Map<IEnumerable<AppointmentReadDto>>(appointments);
            return Result<IEnumerable<AppointmentReadDto>?>.Ok(readDtoList);
        }

        public async Task<Result<IEnumerable<AppointmentReadDto>>?> GetAppointmentsAsyncByPatientId(int DoctorId)
        {
            var appointments = await _unitOfWork.Appointments.FindAsync(x => x.PatientId == DoctorId);
            if (appointments == null || !appointments.Any())
            {
                return Result<IEnumerable<AppointmentReadDto>?>.Fail("No Appoinments", enErrorType.NotFound);
            }

            var readDtoList = _mapper.Map<IEnumerable<AppointmentReadDto>>(appointments);
            return Result<IEnumerable<AppointmentReadDto>?>.Ok(readDtoList);
        }

        public Task<Result<AppointmentReadDto>?> UpdateAsync(int id, AppointmentWriteDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
