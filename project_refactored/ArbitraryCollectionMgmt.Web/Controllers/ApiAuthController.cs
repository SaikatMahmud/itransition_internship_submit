using ArbitraryCollectionMgmt.BLL.MediatorService.AuthMediator;
using ArbitraryCollectionMgmt.BLL.MediatorService.UserMediator;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.BLL.Validation;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.Web.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    [ApiController]
    public class ApiAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ApiAuthController(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        [Route("api/register")]
        [HttpPost]
        public IActionResult Registration(UserRegistrationVM obj)
        {
            UserRegistrationValidator validator = new();
            ValidationResult results = validator.Validate(obj);
            if(!results.IsValid)
            {
                return BadRequest(results.Errors);
            }
            var response = _mediator.Send(new CreateUser.Request(obj)).Result ;
            if (response.Item1)
            {
                return Created();
            }
            return BadRequest(response.Item2);        
        }


        [Route("api/login")]
        [HttpPost]
        public IActionResult Login(LoginVM obj)
        {
            UserLoginValidator validator = new();
            ValidationResult results = validator.Validate(obj);
            if (!results.IsValid)
            {
                return BadRequest(results.Errors);
            }
            var response = _mediator.Send(new UserLogin.Request(obj)).Result;
            if (!string.IsNullOrEmpty(response.Item2))
            {
                return Unauthorized(response.Item2);
            }
            string token = CreateToken(response.Item1.Email, response.Item1.UserId, response.Item1.Name);
            return Ok(new { token });
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
    }
}
