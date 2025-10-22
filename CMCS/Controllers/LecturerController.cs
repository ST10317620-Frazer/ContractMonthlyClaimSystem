using Microsoft.AspNetCore.Mvc;
using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [AuthoriseRole("Lecturer")]
    public class LecturerController : Controller
    {
        private readonly AppDbContext _dbContext;

        public LecturerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var userID = HttpContext.Session.GetInt32("UserID") ?? 0;
            var model = _dbContext.Claims
                .Where(c => c.UserID == userID)
                .ToList();
            return View(model);
        }

        public IActionResult SubmitClaim()
        {
            return View(new Claim { SubmissionDate = DateTime.Now, Status = "Pending" });
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                if (model.SubmissionDate > DateTime.Now)
                {
                    ModelState.AddModelError("SubmissionDate", "Submission date cannot be in the future.");
                    return View(model);
                }

                var userID = HttpContext.Session.GetInt32("UserID");
                if (userID.HasValue)
                {
                    model.UserID = userID.Value;

                    model.TotalAmount = model.HoursWorked * (model.HourlyRate ?? 0);

                    _dbContext.Claims.Add(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "User session expired. Please log in again.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        public IActionResult UploadDocument(int? claimId)
        {
            ViewBag.ClaimId = claimId;
            return View();
        }

        [HttpPost]
        public IActionResult UploadDocument(int claimId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    ViewBag.Error = "No file uploaded.";
                    ViewBag.ClaimId = claimId;
                    return View();
                }

                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ViewBag.Error = "Invalid file type. Please upload .pdf, .docx, or .xlsx files.";
                    ViewBag.ClaimId = claimId;
                    return View();
                }

                if (file.Length > 10 * 1024 * 1024) // 10MB limit
                {
                    ViewBag.Error = "File size exceeds 10MB limit.";
                    ViewBag.ClaimId = claimId;
                    return View();
                }

                var claim = _dbContext.Claims.Find(claimId);
                if (claim == null)
                {
                    ViewBag.Error = "Claim not found.";
                    ViewBag.ClaimId = claimId;
                    return View();
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                claim.DocumentPath = $"/uploads/{fileName}";
                _dbContext.SaveChanges();

                TempData["Message"] = "File uploaded successfully.";
                return RedirectToAction("SubmitClaim", new { id = claimId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred: {ex.Message}";
                ViewBag.ClaimId = claimId;
                return View();
            }
        }
    }
}
