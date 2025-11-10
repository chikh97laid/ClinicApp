using Clinic_Project.Data;
using Clinic_Project.Models;
using Clinic_Project.Repositories.Interfaces;

namespace Clinic_Project.Repositories.Implementations
{
    public class RefreshTokenRepo : MainRepo<RefreshToken>, IRefreshTokenRepo
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepo(AppDbContext context) : base(context) 
        {
            
        }

    }
}
