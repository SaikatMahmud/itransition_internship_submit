using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class ApiTokenService
    {
        private readonly IUnitOfWork DataAccess;
        public ApiTokenService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public ApiTokenDTO Get(Expression<Func<ApiTokenDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ApiToken, ApiTokenDTO>();
                c.CreateMap<User, UserDTO>();
            });
            var mapper = new Mapper(cfg);
            var apiTokenFilter = mapper.MapExpression<Expression<Func<ApiToken, bool>>>(filter);
            var data = DataAccess.ApiToken.Get(apiTokenFilter, properties);
            if (data != null)
            {
                return mapper.Map<ApiTokenDTO>(data);
            }
            return null;
        }

        public List<ApiTokenDTO> GetAll(Expression<Func<ApiTokenDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ApiToken, ApiTokenDTO>();
                c.CreateMap<User, UserDTO>();
            });
            var mapper = new Mapper(cfg);
            var apiTokenFilter = mapper.MapExpression<Expression<Func<ApiToken, bool>>>(filter);
            var data = DataAccess.ApiToken.GetAll(apiTokenFilter, properties);
            if (data != null)
            {
                return mapper.Map<List<ApiTokenDTO>>(data);
            }
            return null;
        }

        public ApiTokenDTO Create(int userId, string tokenName)
        {
            var newObj = new ApiToken()
            {
                TokenKey = Guid.NewGuid().ToString("N"),
                Name = tokenName,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            var data = DataAccess.ApiToken.Create(newObj);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<ApiToken, ApiTokenDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<ApiTokenDTO>(data);
            }
            return null;
        }
        public bool Revoke(int apiTokenId)
        {
            var data = DataAccess.ApiToken.Get(a => a.Id == apiTokenId);
            if (data != null)
            {
                data.IsRevoked = true;
                return DataAccess.ApiToken.Update(data);
            }
            return false;
        }
    }
}
