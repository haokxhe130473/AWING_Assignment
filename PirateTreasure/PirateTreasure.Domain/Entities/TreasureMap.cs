using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateTreasure.Domain.Entities
{
    public class TreasureMap : Entity<Guid>
    {
        public string Name { get; private set; }
        public int Rows { get; private set; }       // n
        public int Columns { get; private set; }    // m
        public int MaxChestValue { get; private set; } // p

        private readonly List<TreasureCell> _cells = new();
        public IReadOnlyCollection<TreasureCell> Cells => _cells;

        // EF constructor
        private TreasureMap()
        { }

        public TreasureMap(string name, int rows, int columns, int maxChestValue)
        {
            Id = Guid.NewGuid();
            Name = name;
            Rows = rows;
            Columns = columns;
            MaxChestValue = maxChestValue;
        }

        public void AddCell(int row, int col, int chestValue)
        {
            _cells.Add(new TreasureCell(row, col, chestValue, this.Id));
        }

        public void SetCells(IEnumerable<TreasureCell> cells)
        {
            _cells.Clear();
            _cells.AddRange(cells);
        }
    }