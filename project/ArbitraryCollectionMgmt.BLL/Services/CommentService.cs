using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.Hubs;
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
    public class CommentService
    {
        private readonly IUnitOfWork DataAccess;
        public CommentService(IUnitOfWork _dataAccess)
        {
            DataAccess = _dataAccess;
        }
        public CommentDTO AddComment(int userId, int itemId, string commentText)
        {
            var commentObj = new Comment()
            {
                UserId = userId,
                ItemId = itemId,
                CommentText = commentText,
                CreatedAt = DateTime.Now
            };
            var data = DataAccess.Comment.Create(commentObj);
            if(data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Comment, CommentDTO>();
                    c.CreateMap<User, UserDTO>();
                });
                var mapper = new Mapper(cfg);
                var comment = mapper.Map<CommentDTO>(data);
                return comment;
            }
            return null;
        }

        public List<CommentDTO> GetComments(int itemId, string? properties = null)
        {       
            var data = DataAccess.Comment.GetAll(c => c.ItemId == itemId, properties);
            if (data != null)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Comment, CommentDTO>();
                    c.CreateMap<User, UserDTO>();
                });
                var mapper = new Mapper(cfg);
                return mapper.Map<List<CommentDTO>>(data);
            }
            return null;
        }
    }
}
