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
            if (request.Rows <= 0 || request.Columns <= 0 || request.MaxChestValue <= 0)
                throw new ValidationException("Số hàng, cột và giá trị p phải lớn hơn 0.");

            if (request.MaxChestValue > request.Rows * request.Columns)
                throw new ValidationException("Giá trị p không được lớn hơn tổng số ô trong bản đồ.");

            if (request.Cells.Count != request.Rows * request.Columns)
                throw new ValidationException("Số lượng ô không khớp với kích thước bản đồ.");

            var startCell = request.Cells.FirstOrDefault(c => c.Row == 1 && c.Col == 1);
            if (startCell is null || startCell.ChestValue != 1)
                throw new ValidationException("Ô (1,1) phải chứa rương số 1 để bắt đầu hành trình.");

            var chestValues = request.Cells.Select(c => c.ChestValue).ToList();

            if (chestValues.Any(v => v < 1 || v > request.MaxChestValue))
                throw new ValidationException($"Tất cả các rương phải có giá trị từ 1 đến {request.MaxChestValue}.");

            for (int v = 1; v <= request.MaxChestValue; v++)
            {
                if (!chestValues.Contains(v))
                    throw new ValidationException($"Thiếu rương có giá trị {v}.");
            }

            // OK, tạo bản đồ
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