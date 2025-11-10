using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Project.Models
{
    public class AppUser : IdentityUser
    {
        [ForeignKey(nameof(Person))]
        public int? PersonId { get; set; }
        public Person? Person { get; set; }

    }
}
