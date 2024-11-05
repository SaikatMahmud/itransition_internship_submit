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
    public class GetCustomizedCollection
    {
        public class CollectionResponse
        {
            public List<CollectionDTO> Collections { get; set; }
            public int TotalCount { get; set; }
            public int FilteredCount { get; set; }
        }

        public record Request(string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? properties = null) : IRequest<CollectionResponse>;
        public class Handler : IRequestHandler<Request, CollectionResponse>
        {
            private readonly IUnitOfWork DataAccess;
            public Handler(IUnitOfWork dataAccess)
            {
                DataAccess = dataAccess;
            }

            public async Task<CollectionResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                var data = DataAccess.Collection.GetAllSortFilterPage(request.searchQuery, request.skip, request.take, request.orderColumn, request.orderDirection, request.properties);
                if (data.Item1 != null)
                {
                    var cfg = new MapperConfiguration(c =>
                    {
                        c.CreateMap<Collection, CollectionDTO>();
                        c.CreateMap<Category, CategoryDTO>();
                        c.CreateMap<User, UserDTO>();

                    });
                    var mapper = new Mapper(cfg);
                    return new CollectionResponse
                    {
                        Collections = mapper.Map<List<CollectionDTO>>(data.Item1),
                        TotalCount = data.Item2,
                        FilteredCount = data.Item3
                    };
                }
                return null;
            }
        }
    }
}
