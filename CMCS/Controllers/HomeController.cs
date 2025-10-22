using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CMCS.Models;

namespace CMCS.Controllers;

public class HomeController : Controller
{
     public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Lecturer")
                return RedirectToAction("Index", "Lecturer");
            if (role == "Admin")
                return RedirectToAction("Index", "Admin");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }

