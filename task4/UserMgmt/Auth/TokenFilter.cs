using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserMgmt.BLL.ServiceAccess;
using UserMgmt.BLL.Services;

namespace UserMgmt.Auth
{
    public class TokenFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private AuthService authService;

        public TokenFilter(IConfiguration configuration, IBusinessService businessService)
        {
            _configuration = configuration;
            authService = businessService.AuthService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = string.Empty;
            token = context.HttpContext.Request.Cookies["access-Token"]?.ToString();
            if (string.IsNullOrEmpty(token) || !IsValidUser(token, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"]))
            {
                //var ReturnUrl = context.HttpContext.Request.Path.Value;
                //context.Result = new RedirectToActionResult("Login", "Auth", new { ReturnUrl });
                context.Result = new RedirectResult("login");
                return;
            }
        }

        public bool IsValidUser(string token, string key, string issuer)
        {
            var principal = ValidateToken(token, key, issuer);
            if (principal == null) return false;
            int userId = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            return authService.IsActiveUser(userId);

        }
        public ClaimsPrincipal ValidateToken(string token, string key, string issuer)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
            };

            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }
    }
}

