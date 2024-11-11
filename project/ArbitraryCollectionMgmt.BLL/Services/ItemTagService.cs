using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class ItemTagService
    {
        private readonly IUnitOfWork DataAccess;
        public ItemTagService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public List<ItemTagDTO> GetAll(Expression<Func<ItemTagDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ItemTag, ItemTagDTO>();
                c.CreateMap<Tag, TagDTO>();
            });
            var mapper = new Mapper(cfg);
            var itemTagFilter = mapper.MapExpression<Expression<Func<ItemTag, bool>>>(filter);
            var data = DataAccess.ItemTag.GetAll(itemTagFilter, properties);
            if (data != null)
            {
                return mapper.Map<List<ItemTagDTO>>(data);
            }
            return null;
        }

        public bool DeleteItemTag(int itemTagId)
        {
            var itemTag = DataAccess.ItemTag.Get(i => i.ItemTagId == itemTagId);
            return DataAccess.ItemTag.Delete(itemTag);
        }
    }
}
