using Microsoft.AspNetCore.Http;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using CMCS.Models;

  namespace CMCS.Services
  {
      public class AuthenticationService : IAuthenticationService
      {
          private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly List<User> _users = new List<User>
          {
              new User { UserID = 1, Username = "BicMitchum", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123"), Role = "Lecturer" },
              new User { UserID = 2, Username = "TannerWillson", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234"), Role = "Admin" }
          };

          public AuthenticationService(IHttpContextAccessor httpContextAccessor)
          {
              _httpContextAccessor = httpContextAccessor;
          }

          public async Task<User> AuthenticateAsync(string username, string password)
          {
              var user = _users.FirstOrDefault(u => u.Username == username && BCrypt.Net.BCrypt.Verify(password, u.PasswordHash));
              if (user != null && _httpContextAccessor.HttpContext != null)
              {
                  var httpContext = _httpContextAccessor.HttpContext;
                  httpContext.Session.SetInt32("UserID", user.UserID);
                  httpContext.Session.SetString("Role", user.Role);
              }
              return await Task.FromResult(user);
          }

          public async Task<User> GetCurrentUserAsync()
          {
              var httpContext = _httpContextAccessor.HttpContext;
              if (httpContext != null)
              {
                  var userID = httpContext.Session.GetInt32("UserID");
                  if (userID.HasValue)
                  {
                      var user = _users.FirstOrDefault(u => u.UserID == userID.Value);
                      return await Task.FromResult(user);
                  }
              }
              return await Task.FromResult<User>(null);
          }
      }
  }