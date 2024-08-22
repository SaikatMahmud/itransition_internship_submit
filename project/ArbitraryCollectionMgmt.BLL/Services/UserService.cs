using AutoMapper;
//using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using System.Linq.Expressions;
using AutoMapper.Extensions.ExpressionMapping;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class UserService
    {
        private readonly IUnitOfWork DataAccess;
        public UserService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public List<UserUserLoginDTO> GetAll(string? properties = null)
        {
            var data = DataAccess.User.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<User, UserUserLoginDTO>();
                    c.CreateMap<UserLogin, UserLoginDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<UserUserLoginDTO>>(data);
            }
            return null;
        }
        public UserDTO Get(Expression<Func<UserDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<User, UserDTO>();
            });
            var mapper = new Mapper(cfg);
            var userFilter = mapper.MapExpression<Expression<Func<User, bool>>>(filter);
            var data = DataAccess.User.Get(userFilter);
            if (data == null) return null;
            return mapper.Map<UserDTO>(data);
        }

        public bool Create(UserRegistrationVM obj, out string errorMsg)
        {
            errorMsg = string.Empty;
            var exixtingUser = DataAccess.User.Get(u => u.Email == obj.Email);
            if (exixtingUser != null)
            {
                if (exixtingUser.UserStatus != "Active") { errorMsg = "Your email is currently blocked!"; return false; }
                errorMsg = "Email already exists!"; return false;
            }
            var user = new User()
            {
                Name = obj.Name,
                Email = obj.Email,
                CreatedAt = DateTime.Now,
                UserStatus = "Active"
            };
            user = DataAccess.User.Create(user, out errorMsg);
            if (user == null) return false;
            var userLogin = new UserLogin()
            {
                UserId = user.UserId,
                Password = obj.Password,
            };
            return DataAccess.UserLogin.Create(userLogin);
        }
        public bool Block(int[] userId)
        {
            var users = DataAccess.User.GetAll(u => userId.Contains(u.UserId));
            if (users == null) return false;
            foreach (var user in users)
            {
                user.UserStatus = "Blocked";
                DataAccess.User.Update(user);
            }
            return true;
        }
        public bool Unblock(int[] userId)
        {
            var users = DataAccess.User.GetAll(u => userId.Contains(u.UserId));
            if (users == null) return false;
            foreach (var user in users)
            {
                user.UserStatus = "Active";
                DataAccess.User.Update(user);
            }
            return true;
        }
        public bool Delete(int[] userId)
        {
            var users = DataAccess.User.GetAll(u => userId.Contains(u.UserId));
            if (users == null) return false;
            return DataAccess.User.DeleteRange(users);
        }
        public bool MakeAdmin(int[] userId)
        {
            var users = DataAccess.User.GetAll(u => userId.Contains(u.UserId));
            if (users == null) return false;
            foreach (var user in users)
            {
                user.IsAdmin = true;
                DataAccess.User.Update(user);
            }
            return true;
        }

        public bool RemoveAdmin(int[] userId)
        {
            var users = DataAccess.User.GetAll(u => userId.Contains(u.UserId));
            if (users == null) return false;
            foreach (var user in users)
            {
                user.IsAdmin = false;
                DataAccess.User.Update(user);
            }
            return true;
        }
    }
}
