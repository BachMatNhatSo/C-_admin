using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AccountController(SignInManager<User> signInManager,UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Get the user details and set the user's name in ViewData
                    var user = await userManager.FindByNameAsync(model.Username);
                    var session = httpContextAccessor.HttpContext.Session;
                    if (user != null)
                    {
                        session.SetString("UserName", user.UserName);
                    }

                    return RedirectToAction("Index", "Saches");
                }
                ModelState.AddModelError("", "Invalid login attempt");
                ViewData["ErrorMessage"] = "Tài khoản hoặc mật khẩu không hợp lệ!!!";
                return View(model);
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                User user = new()
                {
                    Name=model.Name,
                    UserName=model.Email,
                    Email=model.Email,
                };
                var result =await userManager.CreateAsync(user,model.Password!);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login","Account");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            return View();
        }
    }
}
