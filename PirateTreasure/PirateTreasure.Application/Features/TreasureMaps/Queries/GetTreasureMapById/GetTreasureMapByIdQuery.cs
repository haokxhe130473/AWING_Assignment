using MediatR;
using PirateTreasure.Application.Features.TreasureMaps.Dtos;

namespace PirateTreasure.Application.Features.TreasureMaps.Queries.GetTreasureMap
{
    public class GetTreasureMapByIdQuery : IRequest<TreasureMapDto>
    {
        public Guid Id { get; set; }

        public GetTreasureMapByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}