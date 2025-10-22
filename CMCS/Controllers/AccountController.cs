using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Http;
  using CMCS.Services;
  using CMCS.Models;

  namespace CMCS.Controllers
  {
      public class AccountController : Controller
      {
          private readonly IAuthenticationService _authService;

          public AccountController(IAuthenticationService authService)
          {
              _authService = authService;
          }

          [HttpGet]
          public IActionResult Login()
          {
              if (HttpContext.Session.GetInt32("UserId") != null)
                  return RedirectToAction("Index", "Home");
              return View();
          }

          [HttpPost]
          public async Task<IActionResult> Login(LoginViewModel model)
          {
              if (!ModelState.IsValid)
                  return View(model);

              var user = await _authService.AuthenticateAsync(model.Username, model.Password);
              if (user == null)
              {
                  ModelState.AddModelError("", "Invalid username or password");
                  return View(model);
              }

              return RedirectToAction("Index", user.Role == "Lecturer" ? "Lecturer" : "Admin");
          }

          [HttpPost]
          public IActionResult Logout()
          {
              HttpContext.Session.Clear();
              return RedirectToAction("Login");
          }
      }
  }