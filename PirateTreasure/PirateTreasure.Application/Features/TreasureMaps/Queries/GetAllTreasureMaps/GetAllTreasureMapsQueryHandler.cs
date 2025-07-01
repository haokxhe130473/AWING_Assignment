using MediatR;
using PirateTreasure.Application.Contracts.Persistence;
using PirateTreasure.Application.Features.TreasureMaps.Dtos;

namespace PirateTreasure.Application.Features.TreasureMaps.Queries.GetAllTreasureMaps
{
    public class GetAllTreasureMapsQueryHandler : IRequestHandler<GetAllTreasureMapsQuery, List<TreasureMapDto>>
    {
        private readonly ITreasureMapRepository _repository;

        public GetAllTreasureMapsQueryHandler(ITreasureMapRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TreasureMapDto>> Handle(GetAllTreasureMapsQuery request, CancellationToken cancellationToken)
        {
            var maps = await _repository.GetAllAsync();

            return maps.Select(map => new TreasureMapDto
            {
                Id = map.Id,
                Name = map.Name,
                Rows = map.Rows,
                Columns = map.Columns,
                MaxChestValue = map.MaxChestValue,
                Cells = map.Cells.Select(c => new TreasureCellDto
                {
                    Row = c.Row,
                    Col = c.Col,
                    ChestValue = c.ChestValue
                }).ToList()
            }).ToList();
        }
    }
}