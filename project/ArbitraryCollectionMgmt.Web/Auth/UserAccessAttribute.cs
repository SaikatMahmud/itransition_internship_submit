using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Controllers;
using ArbitraryCollectionMgmt.DAL;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;

namespace ArbitraryCollectionMgmt.Auth
{
    public class UserAccessAttribute : TypeFilterAttribute
    {
        public UserAccessAttribute(string? role = "") : base(typeof(JwtFilter))
        {
           Arguments = [role];
        }
    }
}
