using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.Web.Models;
using ArbitraryCollectionMgmt.Web.Clients;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    public class AuthController : Controller
    {
        private AuthService authService;
        private UserService userService;
        private readonly IConfiguration _configuration;
        public AuthController(IBusinessService serviceAccess, IConfiguration configuration)
        {
            authService = serviceAccess.AuthService;
            userService = serviceAccess.UserService;
            _configuration = configuration;
        }
        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginVM obj, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View(obj);
            string errorMsg;
            var user = authService.Authenticate(obj.Email, obj.Password, out errorMsg);
            if (user == null)
            {
                TempData["error"] = errorMsg;
                return View(obj);
            }
            string token = CreateToken(user.Email, user.UserId, user.Name);
            string role = user.IsAdmin ? SD.Role.Admin : SD.Role.User;

            if (token != null)
            {
                SaveTokenAsCookie(token, role, user.Name);
                TempData["success"] = "Login successfully";
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(obj);
        }

        private string CreateToken(string email, int id, string name)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    issuer: _configuration.GetSection("Jwt:Issuer").Value,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Jwt:Lifetime").Value)),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void SaveTokenAsCookie(string token, string role, string name)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(60)
            };
            HttpContext.Response.Cookies.Append("access-Token", token, cookieOptions);
            HttpContext.Response.Cookies.Append("role", role, cookieOptions);
            HttpContext.Response.Cookies.Append("username", name, cookieOptions);
            return;
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
            HttpContext.Response.Cookies.Delete("access-Token");
            HttpContext.Response.Cookies.Delete("role");
            HttpContext.Response.Cookies.Delete("username");
            return RedirectToAction("Login");
        }


        [Route("register")]
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        public IActionResult Registration(UserRegistrationVM obj)
        {
            if (!ModelState.IsValid) return View(obj);
            string errorMsg;
            var result = userService.Create(obj, out errorMsg);
            if (result)
            {
                TempData["success"] = "Registration Success!";
                return RedirectToAction("Login", "Auth");
            }
            TempData["error"] = errorMsg;
            return View(obj);
        }
    }
}
