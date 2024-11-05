using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.CollectionMediator
{
    public class CreateCollection
    {
        public record Request(int userId, CollectionCustomAttributeDTO obj) : IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IUnitOfWork DataAccess;

            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }

            public Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var cfg = new MapperConfiguration(c =>
                {
                    c.CreateMap<CollectionDTO, Collection>();
                    c.CreateMap<CustomAttributeDTO, CustomAttribute>();
                });
                var mapper = new Mapper(cfg);

                var collection = mapper.Map<Collection>(request.obj);
                collection.UserId = request.userId;
                collection.CreatedAt = DateTime.Now;
                collection.UpdatedAt = DateTime.Now;

                var collectionCreated = DataAccess.Collection.Create(collection);
                if (collectionCreated != null)
                {
                    if (request.obj.CustomAttributes != null && request.obj.CustomAttributes.Count() > 0)
                    {
                        foreach (var field in request.obj.CustomAttributes)
                        {
                            var customAttribute = mapper.Map<CustomAttribute>(field);
                            customAttribute.CollectionId = collectionCreated.CollectionId;
                            DataAccess.CustomAttribute.Create(customAttribute);
                        }
                    }
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }
    }
}
