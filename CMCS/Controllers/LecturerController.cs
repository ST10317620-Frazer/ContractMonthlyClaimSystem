using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CMCS.Filters;
using CMCS.Models;

namespace CMCS.Controllers
{
    [AuthoriseRole("Lecturer")]
    public class LecturerController : Controller
    {
        private readonly List<Claim> _claims = new List<Claim>
        {
            new Claim { ClaimID = 1, ClaimName = "Lecture Series A", SubmissionDate = DateTime.Parse("2025-09-10"), HoursWorked = 20, TotalAmount = 2000, Status = "Pending", UserID = 1 },
            new Claim { ClaimID = 2, ClaimName = "Lecture Series B", SubmissionDate = DateTime.Parse("2025-09-12"), HoursWorked = 15, TotalAmount = 1500, Status = "Approved", UserID = 1 },
            new Claim { ClaimID = 3, ClaimName = "Lecture Series C", SubmissionDate = DateTime.Parse("2025-09-15"), HoursWorked = 10, TotalAmount = 1000, Status = "Declined", UserID = 1 }
        };

        public IActionResult Index()
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            var model = _claims.Where(c => c.UserID == userID).ToList();
            return View(model);
        }

        public IActionResult SubmitClaim()
        {
            return View(new Claim { SubmissionDate = DateTime.Now });
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.SubmissionDate > DateTime.Now)
            {
                ModelState.AddModelError("SubmissionDate", "Submission date cannot be in the future.");
                return View(model);
            }
            model.UserID = HttpContext.Session.GetInt32("UserID").Value;
            model.Status = "Pending";
            _claims.Add(model);
            return RedirectToAction("Index");
        }

        public IActionResult UploadDocument()
        {
            return View();
        }
    }
}