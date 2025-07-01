using MediatR;
using PirateTreasure.Application.Contracts.Persistence;
using PirateTreasure.Application.Features.TreasureMaps.Dtos;
using PirateTreasure.Application.Features.TreasureMaps.Queries.GetTreasureMap;

namespace PirateTreasure.Application.Features.TreasureMaps.Queries.GetTreasureMapById
{
    public class GetTreasureMapByIdQueryHandler : IRequestHandler<GetTreasureMapByIdQuery, TreasureMapDto>
    {
        private readonly ITreasureMapRepository _repository;

        public GetTreasureMapByIdQueryHandler(ITreasureMapRepository repository)
        {
            _repository = repository;
        }

        public async Task<TreasureMapDto> Handle(GetTreasureMapByIdQuery request, CancellationToken cancellationToken)
        {
            var map = await _repository.GetByIdAsync(request.Id);
            if (map == null)
                throw new Exception("Treasure map not found");

            return new TreasureMapDto
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
            };
        }
    }
}