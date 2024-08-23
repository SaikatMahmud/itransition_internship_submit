using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class SearchService
    {
        private readonly IUnitOfWork DataAccess;
        public SearchService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }

        public SearchResultDTO GetSearchResult(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery)) return null;
            var data = DataAccess.Search.GetSearchResult(searchQuery);
            if (data == null) return new SearchResultDTO();
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<SearchResult, SearchResultDTO>();
                c.CreateMap<Item, ItemDTO>();
                c.CreateMap<Comment, CommentDTO>();
                c.CreateMap<CustomValue, CustomValueDTO>();
                c.CreateMap<Collection, CollectionDTO>();
            });
            var mapper = new Mapper(cfg);
            return mapper.Map<SearchResultDTO>(data);
        }

        public SearchResultDTO GetSearchResultForTag(int tagId)
        {
            if (tagId == 0) return new SearchResultDTO();
            var data = DataAccess.Search.GetItemWithMatchedTag(tagId);
            if (data == null) return new SearchResultDTO();
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<SearchResult, SearchResultDTO>();
                c.CreateMap<Item, ItemDTO>();
            });
            var mapper = new Mapper(cfg);
            return mapper.Map<SearchResultDTO>(data);
        }

    }
}
