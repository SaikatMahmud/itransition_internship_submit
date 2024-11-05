using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.CollectionMediator
{
    public class UpdateCollection
    {
        public record Request(CollectionCustomAttributeDTO obj) : IRequest<bool>;
        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }
            public Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var existingCollection = DataAccess.Collection.Get(c => c.CollectionId == request.obj.CollectionId);
                if (existingCollection == null)
                {
                    return Task.FromResult(false);
                }
                existingCollection.Name = request.obj.Name;
                existingCollection.Description = request.obj.Description;
                existingCollection.CategoryId = request.obj.CategoryId;
                existingCollection.ImageUrl = request.obj.ImageUrl;
                existingCollection.UpdatedAt = DateTime.Now;
                DataAccess.Collection.Update(existingCollection);
                if (request.obj.CustomAttributes != null && request.obj.CustomAttributes.Count() > 0)
                {
                    foreach (var item in request.obj.CustomAttributes)
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
                                CollectionId = request.obj.CollectionId,
                                FieldName = item.FieldName,
                                FieldType = item.FieldType
                            };
                            DataAccess.CustomAttribute.Create(customAttribute);
                        }
                    }
                }
                return Task.FromResult(true);
            }
        }
    }
}
