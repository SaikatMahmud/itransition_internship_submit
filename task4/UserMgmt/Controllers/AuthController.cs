using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserMgmt.BLL.ServiceAccess;
using UserMgmt.BLL.Services;
using UserMgmt.BLL.ViewModels;
using UserMgmt.DAL.Models;

namespace UserMgmt.Controllers
{
    public class AuthController : Controller
    {
        private AuthService authService;
        private readonly IConfiguration _configuration;
        public AuthController(IBusinessService serviceAccess, IConfiguration configuration)
        {
            authService = serviceAccess.AuthService;
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
        public IActionResult Login(LoginVM obj)
        {
            if (!ModelState.IsValid) return View(obj);
            string errorMsg;
            var user = authService.Authenticate(obj.Email, obj.Password, out errorMsg);
            if(user == null)
            {
                TempData["error"] = errorMsg;
                return View(obj);
            }
            string token = CreateToken(user.Email, user.Id, user.Name);
            if(token != null)
            {
                SaveTokenAsCookie(token);
                TempData["success"] = "Login successfully";
                return RedirectToAction("Index", "Home");
            }
            return View(obj);
        }

        private string CreateToken(string email, int id, string name)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
            };
            HttpContext.Response.Cookies.Append("name", name);
            HttpContext.Response.Cookies.Append("email", email);


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

        private void SaveTokenAsCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(30)
            };
            HttpContext.Response.Cookies.Append("access-Token", token, cookieOptions);
            return;
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Cookies["access-Token"]?.ToString();
            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Response.Cookies.Delete("access-Token");
                HttpContext.Response.Cookies.Delete("name");
                HttpContext.Response.Cookies.Delete("email");

            }
            return RedirectToAction("Login");
        }
    }
}
