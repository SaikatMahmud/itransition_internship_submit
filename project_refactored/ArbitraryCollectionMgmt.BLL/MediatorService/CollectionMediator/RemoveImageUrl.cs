using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.CollectionMediator
{
    public class RemoveImageUrl
    {
        public record Request(int collectionId) : IRequest<bool>;
        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }
            public Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var collection = DataAccess.Collection.Get(c => c.CollectionId == request.collectionId);
                if (collection == null)
                {
                    return Task.FromResult(false);
                }
                var isRemoved = ImageControlService.DeleteCollectionImage(collection.ImageUrl);
                if (!isRemoved)
                {
                    return Task.FromResult(false);
                }
                collection.ImageUrl = string.Empty;
                return Task.FromResult(DataAccess.Collection.Update(collection));
            }
        }
    }
}
