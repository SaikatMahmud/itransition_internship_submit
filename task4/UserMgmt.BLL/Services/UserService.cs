﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgmt.BLL.DTOs;
using UserMgmt.BLL.ViewModels;
using UserMgmt.DAL.Models;
using UserMgmt.DAL.UnitOfWork;

namespace UserMgmt.BLL.Services
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
                UserId = user.Id,
                Password = obj.Password,
            };
            return DataAccess.UserLogin.Create(userLogin);     
        }
        public bool Block(int[] userId)
        {
            var users = DataAccess.User.GetAll(u => userId.Contains(u.Id));
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
            var users = DataAccess.User.GetAll(u => userId.Contains(u.Id));
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
            var users = DataAccess.User.GetAll(u => userId.Contains(u.Id));
            if (users == null) return false;
            return DataAccess.User.DeleteRange(users);
        }
    }
}
