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
            return View(_dbContext.Claims.ToList());
        }

        public IActionResult ClaimDetails(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id);
            if (claim == null)
                return NotFound();
            return View(claim);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id);
            if (claim == null || claim.Status != "Pending")
            {
                ModelState.AddModelError("", "Only pending claims can be approved.");
                return View("ClaimDetails", claim);
            }
            claim.Status = "Approved";
            claim.DateProcessed = DateTime.Now; // this sets the processing date on approval
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeclineClaim(int id)
        {
            var claim = _dbContext.Claims.FirstOrDefault(c => c.ClaimID == id);
            if (claim == null || claim.Status != "Pending")
            {
                ModelState.AddModelError("", "Only pending claims can be declined.");
                return View("ClaimDetails", claim);
            }
            claim.Status = "Declined";
            claim.DateProcessed = DateTime.Now; //this sets processing date on decline
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}