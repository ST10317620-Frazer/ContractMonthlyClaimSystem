using Microsoft.AspNetCore.Mvc;
using CMCS.Data;
using CMCS.Filters;
using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    [AuthoriseRole("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var pendingClaims = _dbContext.Claims.Where(c => c.Status == "Pending").ToList();
            return View(pendingClaims);
        }

        public IActionResult ClaimDetails(int id)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id);
            if (claim == null) return NotFound();
            return View(claim);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            try
            {
                var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id);
                if (claim == null || claim.Status != "Pending")
                {
                    ModelState.AddModelError("", "Only pending claims can be approved.");
                    return View("ClaimDetails", claim);
                }
                claim.Status = "Approved";
                claim.DateProcessed = DateTime.Now;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("ClaimDetails", _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id));
            }
        }

        [HttpPost]
        public IActionResult DeclineClaim(int id)
        {
            try
            {
                var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id);
                if (claim == null || claim.Status != "Pending")
                {
                    ModelState.AddModelError("", "Only pending claims can be declined.");
                    return View("ClaimDetails", claim);
                }
                claim.Status = "Declined";
                claim.DateProcessed = DateTime.Now;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View("ClaimDetails", _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id));
            }
        }
    }
}