using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class HomepageService
    {
        private readonly IUnitOfWork DataAccess;
        public HomepageService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public HomepageVM GetHomepageData(int itemCount, int collectionCount)
        {
            var items = DataAccess.Item.GetRecentlyAdded(itemCount, "Collection, User");
            var collections = DataAccess.Collection.GetTopLargest(collectionCount, "User");

            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Item, ItemDTO>();
                c.CreateMap<Collection, CollectionDTO>();
                c.CreateMap<User, UserDTO>();
            });
            var mapper = new Mapper(cfg);
            var homepageData = new HomepageVM();
            if (items != null) homepageData.Items = mapper.Map<List<ItemDTO>>(items);
            if (collections != null) homepageData.Collections = mapper.Map<List<CollectionDTO>>(collections);
            return homepageData;
        }
    }
}
