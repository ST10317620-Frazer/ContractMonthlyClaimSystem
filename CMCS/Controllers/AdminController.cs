using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CMCS.Data;
using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [Authorize(Roles = "Coordinator,AcademicManager,HR,Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Admin Dashboard shows all pending claims off bat
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Documents)
                .Where(c => c.Status == "Pending")
                .OrderByDescending(c => c.SubmittedDate)
                .ToListAsync();

            return View(claims);
        }

        public async Task<IActionResult> Details(int id)
        {
            var claim = await _context.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null) return NotFound();

            return View(claim);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Approved";
                claim.DateProcessed = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Claim approved successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string rejectionReason)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null && !string.IsNullOrWhiteSpace(rejectionReason))
            {
                claim.Status = "Rejected";
                claim.RejectionReason = rejectionReason;
                claim.DateProcessed = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Claim rejected.";
            }
            return RedirectToAction("Index");
        }
    }
}