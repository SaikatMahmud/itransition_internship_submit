using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.CollectionMediator
{
    public class DeleteCollection
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
                if (collection == null) return Task.FromResult(false);
                var result = DataAccess.Collection.Delete(collection);
                var items = DataAccess.Item.GetAll(i => i.CollectionId == request.collectionId);
                if (items != null)
                {
                    DataAccess.Item.Delete(items);
                }
                return Task.FromResult(result);
            }
        }
    }
}
