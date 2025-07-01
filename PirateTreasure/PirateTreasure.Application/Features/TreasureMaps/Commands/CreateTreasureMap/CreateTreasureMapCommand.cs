using MediatR;
using PirateTreasure.Application.Features.TreasureMaps.Dtos;

namespace PirateTreasure.Application.Features.TreasureMaps.Commands.CreateTreasureMap
{
    public class CreateTreasureMapCommand : IRequest<Guid> // trả về Id của bản đồ
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int MaxChestValue { get; set; }

        public List<TreasureCellDto> Cells { get; set; } = new();
    }
}