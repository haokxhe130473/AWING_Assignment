using PirateTreasure.Domain.Common;

namespace PirateTreasure.Domain.Entities
{
    public class TreasureCell : Entity<Guid>
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int ChestValue { get; private set; }

        public Guid TreasureMapId { get; private set; }

        private TreasureCell()
        { }

        public TreasureCell(int row, int col, int chestValue, Guid treasureMapId)
        {
            Id = Guid.NewGuid();
            Row = row;
            Col = col;
            ChestValue = chestValue;
            TreasureMapId = treasureMapId;
        }
    }
}