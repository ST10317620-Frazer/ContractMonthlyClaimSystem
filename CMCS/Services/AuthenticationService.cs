using CMCS.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMCS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<User> _users = new List<User>
        {
            new User { UserID = "01", Username = "BicMitchum", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123"), Role = "Lecturer" },
            new User { UserID = "02", Username = "TannerWillson", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234"), Role = "Admin" }
        };

        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username && BCrypt.Net.BCrypt.Verify(password, u.PasswordHash));
            if (user != null)
            {
                var 
                httpContext = _httpContextAccessor.HttpContext;
                httpContext.Session.SetString("UserID", user.UserID);
                httpContext.Session.SetString("Role", user.Role);
            }
            return await Task.FromResult(user);
        }

        public async Task<User> GetAuthenticateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userID = httpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userID))
                return null;

            return await Task.FromResult(_users.FirstOrDefault(u => u.UserID == userID));
        }
    }
}
