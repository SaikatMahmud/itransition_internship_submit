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
    public class TagService
    {
        private readonly IUnitOfWork DataAccess;
        public TagService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public List<TagDTO> GetMatchedTags(string searchString)
        {
            var data = DataAccess.Tag.GetMatchedTags(searchString);
            if (data == null) return null;
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Tag, TagDTO>();
            });
            var mapper = new Mapper(cfg);
            return mapper.Map<List<TagDTO>>(data);
        }

        public List<TagDTO> GetAll()
        {
            var data = DataAccess.Tag.GetAll();
            if (data == null) return null;
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Tag, TagDTO>();
            });
            var mapper = new Mapper(cfg);
            return mapper.Map<List<TagDTO>>(data);
        }
    }
}
