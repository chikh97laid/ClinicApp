using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Project.Repositories.Implementations
{
    public class PersonRepo : MainRepo<Person>, IPersonRepo
    {
        private readonly AppDbContext _context;
        public PersonRepo(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.People.AnyAsync(p => p.Email == email);
        }

    }
}
