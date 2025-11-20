using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CMCS.Models;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            // If not logged in → show welcome page
            if (!User.Identity?.IsAuthenticated ?? true)
                return View();

            // Simple, fast role check — no async needed
            if (User.IsInRole("Lecturer"))
                return RedirectToAction("MyClaims", "Claims");

            if (User.IsInRole("Coordinator") || 
                User.IsInRole("AcademicManager") || 
                User.IsInRole("HR") || 
                User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");   // ← GOES TO YOUR ADMIN PAGE

            return View(); // fallback
        }

        [AllowAnonymous] public IActionResult Privacy() => View();
        [AllowAnonymous] public IActionResult AccessDenied() => View();
    }
}