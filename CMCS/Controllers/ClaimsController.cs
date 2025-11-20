using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CMCS.Models;
using CMCS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace CMCS.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Submit() => View();

        [HttpPost]
        public async Task<IActionResult> Submit(decimal HoursWorked, decimal HourlyRate, string? Notes, List<IFormFile> files)
        {
            var claim = new Claim
            {
                LecturerId = _userManager.GetUserId(User),
                HoursWorked = HoursWorked,
                HourlyRate = HourlyRate,
                Notes = Notes
            };

            foreach (var file in files)
            {
                if (file.Length > 5 * 1024 * 1024) continue;
                var ext = Path.GetExtension(file.FileName);
                if (!new[] { ".pdf", ".docx", ".xlsx" }.Contains(ext.ToLower())) continue;

                var fileName = Guid.NewGuid() + ext;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                claim.Documents.Add(new ClaimDocument
                {
                    OriginalFileName = file.FileName,
                    StoredFileName = fileName
                });
            }

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Claim submitted!";
            return RedirectToAction("MyClaims");
        }

        public IActionResult MyClaims()
        {
            var userId = _userManager.GetUserId(User);
            var claims = _context.Claims.Where(c => c.LecturerId == userId).ToList();
            return View(claims);
        }
    }
}