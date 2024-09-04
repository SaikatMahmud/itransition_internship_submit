using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;

namespace ArbitraryCollectionMgmt.Auth
{
    public class JwtFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private AuthService authService;
        private bool isAdminRequired = false;
        private bool isAdmin = false;

        public JwtFilter(string? _role, IConfiguration configuration, IBusinessService businessService)
        {
            _configuration = configuration;
            authService = businessService.AuthService;
            isAdminRequired = (_role == SD.Role.Admin ? true : false);
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;

            var token = string.Empty;
            token = context.HttpContext.Request.Cookies["access-Token"]?.ToString();
            if (string.IsNullOrEmpty(token) || !IsValidUser(token, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], ref context))
            {
                var ReturnUrl = context.HttpContext.Request.Path.Value;
                context.Result = new RedirectToActionResult("Login", "Auth", new { ReturnUrl });
                //context.Result = new RedirectResult("login");
                return;
            }
            else if (isAdminRequired && !isAdmin)
            {
                context.Result = new RedirectResult("/access-denied");
                return;
            }
        }

        public bool IsValidUser(string token, string key, string issuer, ref AuthorizationFilterContext context)
        {
            var principal = ValidateToken(token, key, issuer);
            if (principal == null) return false;
            int userId = Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));
            if(userId == 0) return false;
            context.HttpContext.User = principal;
            return authService.IsValidUser(userId, out isAdmin);

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
