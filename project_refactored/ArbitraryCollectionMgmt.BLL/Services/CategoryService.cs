using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork DataAccess;
        public CategoryService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public List<CategoryDTO> GetAll(string? properties = null)
        {
            var data = DataAccess.Category.GetAll(properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Category, CategoryDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<CategoryDTO>>(data);

            }
            return null;
        }  
    }
}
