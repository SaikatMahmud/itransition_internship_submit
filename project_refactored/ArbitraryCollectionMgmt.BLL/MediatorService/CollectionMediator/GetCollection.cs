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
    public class GetCollection
    {
        public record Request(Expression<Func<CollectionDTO, bool>> filter, string? properties = null) : IRequest<CollectionCustomAttributeDTO>;
        public class Handler : IRequestHandler<Request, CollectionCustomAttributeDTO>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }
            public Task<CollectionCustomAttributeDTO> Handle(Request request, CancellationToken cancellationToken)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<Collection, CollectionCustomAttributeDTO>();
                    c.CreateMap<CustomAttribute, CustomAttributeDTO>();
                    c.CreateMap<Collection, CollectionDTO>();
                });

                var mapper = new Mapper(cfg);
                var collectionFilter = mapper.MapExpression<Expression<Func<Collection, bool>>>(request.filter);
                var data = DataAccess.Collection.Get(collectionFilter, request.properties);
                if (data != null)
                {
                    return Task.FromResult(mapper.Map<CollectionCustomAttributeDTO>(data));
                }
                return null;
            }
        }
    }
}
