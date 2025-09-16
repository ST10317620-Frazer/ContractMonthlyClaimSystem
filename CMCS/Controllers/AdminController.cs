using Microsoft.AspNetCore.Mvc;
namespace CMCS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult ClaimDetails() => View();
    }
}