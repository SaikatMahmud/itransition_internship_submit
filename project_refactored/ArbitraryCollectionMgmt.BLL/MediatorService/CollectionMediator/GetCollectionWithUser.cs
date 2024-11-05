using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.CollectionMediator
{
    public class GetCollectionWithUser
    {
        public record Request(Expression<Func<CollectionDTO, bool>> filter) : IRequest<CollectionDTO>;

        public class Handler : IRequestHandler<Request, CollectionDTO>
        {
            private readonly IUnitOfWork DataAccess;

            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }
            public Task<CollectionDTO> Handle(Request request, CancellationToken cancellationToken)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Collection, CollectionDTO>();
                    c.CreateMap<User, UserDTO>();
                });

                var mapper = new Mapper(cfg);
                var collectionFilter = mapper.MapExpression<Expression<Func<Collection, bool>>>(request.filter);
                var data = DataAccess.Collection.Get(collectionFilter, "User");
                if (data != null)
                {
                    return Task.FromResult(mapper.Map<CollectionDTO>(data));
                }
                return Task.FromResult<CollectionDTO>(null);

            }
        }
    }
}
