using Microsoft.AspNetCore.Identity;

namespace ContractLecturerClaimSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? EmployeeNumber { get; set; }
    }
}