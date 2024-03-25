using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteAdmin.Data;
using WebsiteAdmin.Models;
using WebsiteAdmin.Services;

namespace WebsiteAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;

        public AuthController(SignInManager<User> signInManager, JwtService jwtService)
        {
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // User successfully authenticated, generate JWT token
                    var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
                    var token = _jwtService.GenerateJwtToken(user.Id);

                    // Return the JWT token as part of the response
                    return Ok(token);
                }
                return BadRequest("Invalid login attempt");
            }
            return BadRequest(ModelState);
        }
    }

}
