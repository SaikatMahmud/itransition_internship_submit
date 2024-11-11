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
    public class ItemService
    {
        private readonly IUnitOfWork DataAccess;
        public ItemService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }  

        public List<ItemDTO> GetCustomized(string searchQuery, int skip, int take, string orderColumn, string orderDirection, out int totalCount, out int filteredCount, string? properties = null)
        {
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Item.GetAllSortFilterPage(searchQuery, skip, take, orderColumn, orderDirection, properties);
            if (data.Item1 != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Item, ItemDTO>();
                    c.CreateMap<Collection, CollectionDTO>();
                    c.CreateMap<User, UserDTO>();
                });
                var mapper = new Mapper(cfg);
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ItemDTO>>(data.Item1);

            }
            return null;
        }

        public List<ItemDTO> GetCustomized(Expression<Func<ItemDTO, bool>> filter, string searchQuery, int skip, int take, string orderColumn, string orderDirection, out int totalCount, out int filteredCount, string? properties = null)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Item, ItemDTO>();
                c.CreateMap<Collection, CollectionDTO>();
            });
            var mapper = new Mapper(cfg);
            var itemFilter = mapper.MapExpression<Expression<Func<Item, bool>>>(filter);
            totalCount = 0;
            filteredCount = 0;
            var data = DataAccess.Item.GetAllSortFilterPage(itemFilter, searchQuery, skip, take, orderColumn, orderDirection, properties);
            if (data.Item1 != null)
            {
                totalCount = data.Item2;
                filteredCount = data.Item3;
                return mapper.Map<List<ItemDTO>>(data.Item1);

            }
            return null;
        }
    
        public ItemCustomAttributeValueDTO ViewItem(int itemId)
        {
            //var data = DataAccess.Item.Get(i => i.ItemId == itemId, "CustomValues.CustomAttribute");
            var item = DataAccess.Item.Get(i => i.ItemId == itemId, "CustomValues");
            if (item == null) return null;
            var customValues = item.CustomValues.ToList();
            var customAttributes = DataAccess.CustomAttribute.GetAll(c => c.CollectionId == item.CollectionId);

            item.CustomValues = (from ca in customAttributes //right joining to get all custom attributes, in case of no custom value available
                                 join cv in customValues
                                 on ca.Id equals cv.CustomAttributeId into cvs
                                 select new CustomValue
                                 {
                                     Id = cvs.FirstOrDefault()?.Id ?? 0,
                                     ItemId = cvs.FirstOrDefault()?.ItemId ?? 0,
                                     CustomAttributeId = ca.Id,
                                     FieldValue = cvs.FirstOrDefault()?.FieldValue,
                                     CustomAttribute = ca,
                                 }).ToList();

            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Item, ItemCustomAttributeValueDTO>();
                c.CreateMap<CustomAttribute, CustomAttributeDTO>();
                c.CreateMap<CustomValue, CustomValueDTO>();
            });
            var mapper = new Mapper(cfg);
            return mapper.Map<ItemCustomAttributeValueDTO>(item);
        }

        public bool CreateCustomValue(int itemId, List<CustomValueDTO> objs)
        {
            if (objs != null && objs.Count() > 0)
            {
                foreach (var value in objs)
                {
                    if (string.IsNullOrEmpty(value.FieldValue)) continue;
                    var customValue = new CustomValue()
                    {
                        ItemId = itemId,
                        CustomAttributeId = value.CustomAttributeId,
                        FieldValue = value.FieldValue
                    };
                    DataAccess.CustomValue.Create(customValue);
                }
                return true;
            }
            return false;
        }
        public bool CreateItemTag(int itemId, int[] tagIds)
        {
            if (tagIds != null && tagIds.Length > 0)
            {
                foreach (var tagId in tagIds)
                {
                    var itemTag = new ItemTag()
                    {
                        ItemId = itemId,
                        TagId = tagId
                    };
                    DataAccess.ItemTag.Create(itemTag);
                }
                return true;
            }
            return false;
        }
        public bool CreateUserProvidedTag(int itemId, string[] userProvidedTags)
        {
            if (userProvidedTags != null && userProvidedTags.Length > 0)
            {
                foreach (var tag in userProvidedTags)
                {
                    var newTag = new Tag()
                    {
                        Name = tag
                    };
                    var tagCreated = DataAccess.Tag.Create(newTag);
                    if (tagCreated != null)
                    {
                        var itemTag = new ItemTag()
                        {
                            ItemId = itemId,
                            TagId = tagCreated.TagId
                        };
                        DataAccess.ItemTag.Create(itemTag);
                    }
                }
                return true;
            }
            return false;
        }


        public bool Create(int userId, ItemCustomAttributeValueDTO obj, int[] tagIds, string[] userProvidedTags)
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ItemDTO, Item>();
            });
            var mapper = new Mapper(cfg);
            var item = mapper.Map<Item>(obj);
            item.OwnedBy = userId;
            item.CreatedAt = DateTime.Now;
            item.UpdatedAt = DateTime.Now;
            var itemCreated = DataAccess.Item.Create(item);
            if (itemCreated != null)
            {
                CreateCustomValue(itemCreated.ItemId, obj.CustomValues);               
                CreateItemTag(itemCreated.ItemId, tagIds);               
                CreateUserProvidedTag(itemCreated.ItemId, userProvidedTags);
                return true;
            }
            return false;
        }

        public bool Update(ItemCustomAttributeValueDTO obj, int[] tagIds, string[] userProvidedTags)
        {
            var existingItem = DataAccess.Item.Get(c => c.ItemId == obj.ItemId);
            if (existingItem == null) return false;
            existingItem.Name = obj.Name;
            existingItem.UpdatedAt = DateTime.Now;
            DataAccess.Item.Update(existingItem);
            if (obj.CustomValues != null && obj.CustomValues.Count() > 0)
            {
                foreach (var field in obj.CustomValues)
                {
                    var existingValue = DataAccess.CustomValue.Get(c => c.Id == field.Id);
                    if (existingValue != null)
                    {
                        existingValue.FieldValue = field.FieldValue;
                        DataAccess.CustomValue.Update(existingValue);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(field.FieldValue)) continue;
                        var customValue = new CustomValue()
                        {
                            ItemId = obj.ItemId,
                            CustomAttributeId = field.CustomAttributeId,
                            FieldValue = field.FieldValue
                        };
                        DataAccess.CustomValue.Create(customValue);
                    }
                }
            }
            CreateItemTag(existingItem.ItemId, tagIds);            
            CreateUserProvidedTag(existingItem.ItemId, userProvidedTags);
            return true;
        }
        public bool Delete(int itemId)
        {
            var item = DataAccess.Item.Get(i => i.ItemId == itemId);
            if (item == null) return false;
            return DataAccess.Item.Delete(item);
        }  
    }
}
