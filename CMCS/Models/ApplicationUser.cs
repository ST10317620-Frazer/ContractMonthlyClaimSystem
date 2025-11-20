using Microsoft.AspNetCore.Identity;

namespace CMCS.Models
{
   public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = "";
    }
}