using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteAdmin.Models;

namespace WebsiteAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        public AccountController(SignInManager<User> signInManager,UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
                var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!,model.RememberMe,false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Saches");
                }
                ModelState.AddModelError("", "Invalid login attemp");
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
