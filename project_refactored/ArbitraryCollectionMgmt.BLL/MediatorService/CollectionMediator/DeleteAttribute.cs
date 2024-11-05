using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.MediatorService.CollectionMediator
{
    public class DeleteAttribute
    {
        public record Request(int attributeId) : IRequest<bool>;
        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }
            public Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var attribute = DataAccess.CustomAttribute.Get(a => a.Id == request.attributeId);
                if (attribute == null)
                {
                    return Task.FromResult(false);
                }
                return Task.FromResult(DataAccess.CustomAttribute.Delete(attribute));
            }
        }
    }
}
