using MediatR;
using PirateTreasure.Application.Contracts.Persistence;
using PirateTreasure.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PirateTreasure.Application.Features.TreasureMaps.Commands.CreateTreasureMap
{
    public class CreateTreasureMapCommandHandler : IRequestHandler<CreateTreasureMapCommand, Guid>
    {
        private readonly ITreasureMapRepository _treasureMapRepository;

        public CreateTreasureMapCommandHandler(ITreasureMapRepository treasureMapRepository)
        {
            _treasureMapRepository = treasureMapRepository;
        }

        public async Task<Guid> Handle(CreateTreasureMapCommand request, CancellationToken cancellationToken)
        {
            var startCell = request.Cells.FirstOrDefault(c => c.Row == 1 && c.Col == 1);
            if (startCell is null || startCell.ChestValue != 1)
                throw new ValidationException("Ô (1,1) phải chứa rương số 1 để bắt đầu hành trình.");

            var treasureMap = new TreasureMap(request.Name, request.Rows, request.Columns, request.MaxChestValue);

            foreach (var cell in request.Cells)
            {
                treasureMap.AddCell(cell.Row, cell.Col, cell.ChestValue);
            }

            await _treasureMapRepository.AddAsync(treasureMap);
            await _treasureMapRepository.SaveChangesAsync();

            return treasureMap.Id;
        }
    }
}