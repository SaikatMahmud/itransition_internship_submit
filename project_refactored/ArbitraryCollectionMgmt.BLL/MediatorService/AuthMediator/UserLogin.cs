using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.AuthMediator
{
    public class UserLogin
    {
        public record Request(LoginVM obj) : IRequest<(UserDTO, string)>;
        public class Handler : IRequestHandler<Request, (UserDTO, string)>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork _dataAccess)
            {
                DataAccess = _dataAccess;
            }
            public Task<(UserDTO, string)> Handle(Request request, CancellationToken cancellationToken)
            {
                string errorMsg = string.Empty;
                var user = DataAccess.UserLogin.Authenticate(request.obj.Email, request.obj.Password);
                if (user == null)
                {
                    errorMsg = "Invalid email or password!";
                    return Task.FromResult((new UserDTO(), errorMsg));
                }
                if (user.UserStatus != "Active")
                {
                    errorMsg = "You are currently blocked!";
                    return Task.FromResult((new UserDTO(), errorMsg));
                }
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<User, UserDTO>();
                });
                var mapper = new Mapper(cfg);
                return Task.FromResult((mapper.Map<UserDTO>(user), errorMsg));
            }
        }
    }
}
