using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class CollectionService
    {
        private readonly IUnitOfWork DataAccess;
        public CollectionService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public UserCollectionDataDTO GetUserCollectionData(int userId) // created for API
        {
            var data = DataAccess.Collection.GetAll(c => c.UserId == userId, "Category, Items, User");
            if (data == null) return null;
            var userCollectionData = new UserCollectionDataDTO();
            userCollectionData.Owner = data.FirstOrDefault().User.Name;
            userCollectionData.Collections = new List<CollectionData>();
            foreach (var collection in data)
            {
                var collectionData = new CollectionData()
                {
                    Name = collection.Name,
                    Description = collection.Description,
                    ItemCount = collection.Items.Count(),
                    Category = collection.Category.Name
                };
                userCollectionData.Collections.Add(collectionData);
            }
            return userCollectionData;

        }

        public CollectionCustomAttributeDTO Get(Expression<Func<CollectionDTO, bool>> filter, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Collection, CollectionCustomAttributeDTO>();
                c.CreateMap<CustomAttribute, CustomAttributeDTO>();
                c.CreateMap<Collection, CollectionDTO>();
            });

            var mapper = new Mapper(cfg);
            var collectionFilter = mapper.MapExpression<Expression<Func<Collection, bool>>>(filter);
            var data = DataAccess.Collection.Get(collectionFilter, properties);
            if (data != null)
            {
                return mapper.Map<CollectionCustomAttributeDTO>(data);
            }
            return null;
        }

        public CollectionDTO GetCollectionWithUser(Expression<Func<CollectionDTO, bool>> filter)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Collection, CollectionDTO>();
                c.CreateMap<User, UserDTO>();
            });

            var mapper = new Mapper(cfg);
            var collectionFilter = mapper.MapExpression<Expression<Func<Collection, bool>>>(filter);
            var data = DataAccess.Collection.Get(collectionFilter, "User");
            if (data != null)
            {
                return mapper.Map<CollectionDTO>(data);
            }
            return null;
        }

        public bool Create(int userId, CollectionCustomAttributeDTO obj)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<CollectionDTO, Collection>();
                c.CreateMap<CustomAttributeDTO, CustomAttribute>();
            });
            var mapper = new Mapper(cfg);
            var collection = mapper.Map<Collection>(obj);
            collection.UserId = userId;
            collection.CreatedAt = DateTime.Now;
            collection.UpdatedAt = DateTime.Now;
            var collectionCreated = DataAccess.Collection.Create(collection);
            if (collectionCreated != null)
            {
                if (obj.CustomAttributes != null && obj.CustomAttributes.Count() > 0)
                {
                    foreach (var field in obj.CustomAttributes)
                    {
                        var customAttribute = mapper.Map<CustomAttribute>(field);
                        customAttribute.CollectionId = collectionCreated.CollectionId;
                        var createdAttribute = DataAccess.CustomAttribute.Create(customAttribute);
                    }
                }
                return true;
            }
            return false;
        }


        public bool Update(CollectionCustomAttributeDTO obj)
        {
            var existingCollection = DataAccess.Collection.Get(c => c.CollectionId == obj.CollectionId);
            if (existingCollection == null) return false;
            existingCollection.Name = obj.Name;
            existingCollection.Description = obj.Description;
            existingCollection.CategoryId = obj.CategoryId;
            existingCollection.ImageUrl = obj.ImageUrl;
            existingCollection.UpdatedAt = DateTime.Now;
            DataAccess.Collection.Update(existingCollection);
            if (obj.CustomAttributes != null && obj.CustomAttributes.Count() > 0)
            {
                foreach (var item in obj.CustomAttributes)
                {
                    var existingAttribute = DataAccess.CustomAttribute.Get(c => c.Id == item.Id);
                    if (existingAttribute != null)
                    {
                        existingAttribute.FieldName = item.FieldName;
                        existingAttribute.FieldType = item.FieldType;
                        DataAccess.CustomAttribute.Update(existingAttribute);
                    }
                    else
                    {
                        var customAttribute = new CustomAttribute()
                        {
                            CollectionId = obj.CollectionId,
                            FieldName = item.FieldName,
                            FieldType = item.FieldType
                        };
                        DataAccess.CustomAttribute.Create(customAttribute);
                    }
                }
            }
            return true;
        }
        public bool DeleteAttribute(int attributeId)
        {
            var attribute = DataAccess.CustomAttribute.Get(a => a.Id == attributeId);
            if (attribute == null) return false;
            return DataAccess.CustomAttribute.Delete(attribute);
        }

        public bool RemoveImageUrl(int collectionId)
        {
            var collection = DataAccess.Collection.Get(c => c.CollectionId == collectionId);
            if (collection == null) return false;
            var isRemoved = ImageControlService.DeleteCollectionImage(collection.ImageUrl);
            if (!isRemoved) return false;
            collection.ImageUrl = string.Empty;
            return DataAccess.Collection.Update(collection);
        }
        public bool Delete(int collectionId)
        {
            var collection = DataAccess.Collection.Get(c => c.CollectionId == collectionId);
            if (collection == null) return false;
            var result = DataAccess.Collection.Delete(collection);
            var items = DataAccess.Item.GetAll(i => i.CollectionId == collectionId);
            if (items != null)
            {
                DataAccess.Item.Delete(items);
            }
            return result;
        }


        public List<CollectionDTO> GetCustomized(string searchQuery, int skip, int take, string orderColumn, string orderDirection, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Collection.GetAllSortFilterPage(searchQuery, skip, take, orderColumn, orderDirection, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Collection, CollectionDTO>();
                    c.CreateMap<Category, CategoryDTO>();
                    c.CreateMap<User, UserDTO>();

                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CollectionDTO>>(data.Item1);

            }
            return null;
        }

        public List<CollectionDTO> GetCustomized(Expression<Func<CollectionDTO, bool>> filter, string searchQuery, int skip, int take, string orderColumn, string orderDirection, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Collection, CollectionDTO>();
                c.CreateMap<Category, CategoryDTO>();
                c.CreateMap<User, UserDTO>();

            });
            var mapper = new Mapper(cfg);
            var productFilter = mapper.MapExpression<Expression<Func<Collection, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Collection.GetAllSortFilterPage(productFilter, searchQuery, skip, take, orderColumn, orderDirection, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<CollectionDTO>>(data.Item1);

            }
            return null;
        }
    }
}
