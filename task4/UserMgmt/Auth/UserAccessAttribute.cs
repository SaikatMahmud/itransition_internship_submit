using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using UserMgmt.BLL.ServiceAccess;
using UserMgmt.BLL.Services;
using UserMgmt.Controllers;
using UserMgmt.DAL;
using UserMgmt.DAL.UnitOfWork;

namespace UserMgmt.Auth
{
    public class UserAccessAttribute : TypeFilterAttribute
    {
        public UserAccessAttribute() : base(typeof(TokenFilter))
        {
           
        }
    }
}
