using Microsoft.AspNetCore.Mvc;
namespace CMCS.Controllers
{
    public class LecturerController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult SubmitClaim() => View();
        public IActionResult UploadDocument() => View();
    }
}