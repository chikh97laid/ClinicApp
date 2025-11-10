namespace Clinic_Project.Dtos.Account
{
    public class AccountDto
    {
        public string Id { get; set; } = string.Empty;          
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<string>? Roles { get; set; }
    }

}
