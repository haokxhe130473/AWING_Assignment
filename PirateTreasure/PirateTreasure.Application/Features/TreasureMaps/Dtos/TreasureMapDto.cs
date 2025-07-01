namespace PirateTreasure.Application.Features.TreasureMaps.Dtos
{
    public class TreasureMapDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int MaxChestValue { get; set; }
        public List<TreasureCellDto> Cells { get; set; } = new();
    }
}