using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.UserMediator
{
    public class CreateUser
    {
        public record Request(UserRegistrationVM obj) : IRequest<(bool, string)>;
        public class Handler : IRequestHandler<Request, (bool, string)>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }
            public Task<(bool, string)> Handle(Request request, CancellationToken cancellationToken)
            {
                string errorMsg = string.Empty;
                var exixtingUser = DataAccess.User.Get(u => u.Email == request.obj.Email);
                if (exixtingUser != null)
                {
                    if (exixtingUser.UserStatus != "Active") { errorMsg = "Your email is currently blocked!"; return Task.FromResult((false, errorMsg)); }
                    errorMsg = "Email already exists!"; return Task.FromResult((false, errorMsg));
                }
                var user = new User()
                {
                    Name = request.obj.Name,
                    Email = request.obj.Email,
                    CreatedAt = DateTime.Now,
                    UserStatus = "Active"
                };
                user = DataAccess.User.Create(user, out errorMsg);
                if (user == null) return Task.FromResult((false, errorMsg));
                var userLogin = new UserLogin()
                {
                    UserId = user.UserId,
                    Password = request.obj.Password,
                };
                return Task.FromResult((DataAccess.UserLogin.Create(userLogin), errorMsg));
            }
        }
    }
}
