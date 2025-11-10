using AutoMapper;
using Clinic_Project.Dtos.Appointment;
using Clinic_Project.Dtos.Doctor;
using Clinic_Project.Dtos.Medication;
using Clinic_Project.Dtos.Patient;
using Clinic_Project.Dtos.Payment;
using Clinic_Project.Dtos.PaymentMethod;
using Clinic_Project.Dtos.Person;
using Clinic_Project.Dtos.Prescription;
using Clinic_Project.Dtos.PrescriptionMedication;
using Clinic_Project.Dtos.Record;
using Clinic_Project.Dtos.Specialization;
using Clinic_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Clinic_Project.Profiles
{
    public class AllProfiles : Profile
    {
        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            var age = today.Year - birthDate.Year;

            return age;
        }

        private string FullName(string firstName, string lastName)
        {
            return firstName + " " + lastName;
        }

        public AllProfiles()
        {
            CreateMap<Person, PersonReadDto>().ForMember(des => des.Age, op => op.MapFrom(p => CalculateAge(p.DateOfBirth)));
            CreateMap<PersonWriteDto, Person>();

            CreateMap<Patient, PatientReadDto>()
                .ForMember(des => des.FirstName, op => op.MapFrom(src => src.Person.FirstName))
                .ForMember(des => des.LastName, op => op.MapFrom(src => src.Person.LastName))
                .ForMember(des => des.Email, op => op.MapFrom(src => src.Person.Email))
                .ForMember(des => des.Phone, op => op.MapFrom(src => src.Person.Phone))
                .ForMember(des => des.Address, op => op.MapFrom(src => src.Person.Address))
                .ForMember(des => des.Age, op => op.MapFrom(src => CalculateAge(src.Person.DateOfBirth)))
                .ForMember(des => des.Gender, op => op.MapFrom(src => src.Person.Gender));
            CreateMap<PatientWriteDto, Person>();


            CreateMap<Doctor, DoctorReadDto>()
                .ForMember(des => des.FirstName, op => op.MapFrom(src => src.Person.FirstName))
                .ForMember(des => des.LastName, op => op.MapFrom(src => src.Person.LastName))
                .ForMember(des => des.Email, op => op.MapFrom(src => src.Person.Email))
                .ForMember(des => des.Phone, op => op.MapFrom(src => src.Person.Phone))
                .ForMember(des => des.Address, op => op.MapFrom(src => src.Person.Address))
                .ForMember(des => des.Age, op => op.MapFrom(src => CalculateAge(src.Person.DateOfBirth)))
                .ForMember(des => des.Gender, op => op.MapFrom(src => src.Person.Gender))
                .ForMember(des => des.Specialization, op => op.MapFrom(src => src.Specialization.Name));
            CreateMap<DoctorWriteDto, Person>();


            CreateMap<Appointment, AppointmentReadDto>()
                .ForMember(des => des.CreatedAt, op => op.MapFrom(src => src.CreatedAt.ToLocalTime()))
                .ForMember(des => des.PatientName, op => op.MapFrom(src => FullName(src.Patient.Person.FirstName, src.Patient.Person.LastName)))
                .ForMember(des => des.DoctorName, op => op.MapFrom(src => FullName(src.Doctor.Person.FirstName, src.Doctor.Person.LastName)))
                .ForMember(des => des.Specialization, op => op.MapFrom(src => src.Doctor.Specialization.Name));
            CreateMap<AppointmentWriteDto, Appointment>()
                .ForMember(des => des.CreatedAt, op => op.MapFrom(src => DateTime.UtcNow));

            CreateMap<Appointment, AppointmentConfirmedReadDto>()
                .ForMember(des => des.PatientName, op => op.MapFrom(src => FullName(src.Patient.Person.FirstName, src.Patient.Person.LastName)))
                .ForMember(des => des.DoctorName, op => op.MapFrom(src => FullName(src.Doctor.Person.FirstName, src.Doctor.Person.LastName)));


            CreateMap<Prescription, PrescriptionReadDto>()
                .ForMember(des => des.PatientName, op => op.MapFrom(src => FullName(src.Patient.Person.FirstName, src.Patient.Person.LastName)))
                .ForMember(des => des.DoctorName, op => op.MapFrom(src => FullName(src.Doctor.Person.FirstName, src.Doctor.Person.LastName)))
                .ForMember(des => des.Specialization, op => op.MapFrom(src => src.Doctor.Specialization.Name))
                .ForMember(des => des.PatientAge, op => op.MapFrom(src => CalculateAge(src.Patient.Person.DateOfBirth)))
                .ForMember(des => des.Record, op => op.MapFrom(src => new RecordReadDto()
                {
                    Id = src.Record.Id,
                    Date = src.Record.Date,
                    Description = src.Record.Description,
                    Diagnosis = src.Record.Diagnosis,
                    DoctorId = src.Record.DoctorId,
                    DoctorName = FullName(src.Doctor.Person.FirstName, src.Doctor.Person.LastName),
                    PatientName = FullName(src.Patient.Person.FirstName, src.Patient.Person.LastName),
                    PatientAge = CalculateAge(src.Patient.Person.DateOfBirth),
                    PatientId = src.PatientId,
                    Specialization = src.Doctor.Specialization.Name
                }))
                .ForMember(des => des.Medications, op => op.MapFrom(src => src.Medications.Select(pm => new PrescriptionMedicationReadDto()
                {
                    Id = pm.Id,
                    MedicationName = pm.Medication.Name,
                    Dosage = pm.Dosage,
                    Frequency = pm.Frequency,
                    StartDate = pm.StartDate,
                    EndDate = pm.EndDate,
                    MedicationId = pm.MedicationId,
                    PrescriptionId = src.Id
                })));

            CreateMap<PrescriptionWriteDto, Prescription>()
                .ForMember(des => des.IssueDate, op => op.MapFrom( _ => DateTime.Now))
                .ForMember(des => des.Record, op => op.MapFrom(src => new Record()
                {
                    Date = DateTime.Now,
                    Diagnosis = src.Record.Diagnosis,
                    Description = src.Record.Description,
                    PatientId = src.PatientId,   
                    DoctorId = src.DoctorId
                }))
                .ForMember(des => des.Medications, op => op.MapFrom(src => src.Medications.Select(pm => new PrescriptionMedication()
                {
                    Dosage = pm.Dosage,
                    Frequency = pm.Frequency,
                    StartDate = pm.StartDate,
                    EndDate = pm.EndDate,
                    MedicationId = pm.MedicationId
                })));

            CreateMap<PrescriptionUpdateWriteDto, Prescription>();

            CreateMap<Record, RecordReadDto>()
                .ForMember(des => des.PatientName, op => op.MapFrom(src => FullName(src.Patient.Person.FirstName, src.Patient.Person.LastName)))
                .ForMember(des => des.DoctorName, op => op.MapFrom(src => FullName(src.Doctor.Person.FirstName, src.Doctor.Person.LastName)))
                .ForMember(des => des.Specialization, op => op.MapFrom(src => src.Doctor.Specialization.Name))
                .ForMember(des => des.PatientAge, op => op.MapFrom(src => CalculateAge(src.Patient.Person.DateOfBirth)));
            CreateMap<RecordWriteDto, Record>();

            CreateMap<RecordUpdateWriteDto, Record>();

            CreateMap<Medication, MedicationReadDto>();
            CreateMap<MedicationWriteDto, Medication>();

            CreateMap<PrescriptionMedication, PrescriptionMedicationReadDto>()
                .ForMember(des => des.MedicationName, op => op.MapFrom(src => src.Medication.Name));
            CreateMap<PrescriptionMedicationWriteDto, PrescriptionMedication>();

            CreateMap<PrescriptionMedicationBaseDto, PrescriptionMedication>();

            CreateMap<Payment, PaymentReadDto>()
                .ForMember(des => des.Date, op => op.MapFrom(src => src.Date.ToLocalTime()))
                .ForMember(des => des.PatientName, op => op.MapFrom(src => FullName(src.Appointment.Patient.Person.FirstName, src.Appointment.Patient.Person.LastName)))
                .ForMember(des => des.DoctorName, op => op.MapFrom(src => FullName(src.Appointment.Doctor.Person.FirstName, src.Appointment.Doctor.Person.LastName)))
                .ForMember(des => des.Specialization, op => op.MapFrom(src => src.Appointment.Doctor.Specialization.Name))
                .ForMember(des => des.PaymentMethodName, op => op.MapFrom(src => src.PaymentMethod.Name));         
           
            CreateMap<PaymentWriteDto, Payment>()
                .ForMember(des => des.Date, op => op.MapFrom(src => DateTime.UtcNow));

            CreateMap<PaymentUpdateWriteDto, Payment>();
            
            CreateMap<PaymentMethod, PaymentMethodReadDto>();
            CreateMap<PaymentMethodWriteDto, PaymentMethod>();

            CreateMap<Specialization, SpecializationReadDto>();
            CreateMap<SpecializationWriteDto, Specialization>();

        }
    }
}
