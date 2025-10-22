using CMCS.Models;
using System.Threading.Tasks;

namespace CMCS.Services
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserAsync();
     }
}