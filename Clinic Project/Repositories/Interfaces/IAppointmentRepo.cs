using Clinic_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Repositories.Interfaces
{
    public interface IAppointmentRepo : IRepo<Appointment>
    {
        public void UpdateAppointmentStatus(Appointment appointment);
    }
}
