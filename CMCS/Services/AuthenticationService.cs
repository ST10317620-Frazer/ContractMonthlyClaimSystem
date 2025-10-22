using CMCS.Models;

namespace CMCS.Services
{
    public interface IAuthenticationService
    {
        Task<User> authenticateAsync(string username, string password);
        Task<User> GetUserAsync();
     }
}