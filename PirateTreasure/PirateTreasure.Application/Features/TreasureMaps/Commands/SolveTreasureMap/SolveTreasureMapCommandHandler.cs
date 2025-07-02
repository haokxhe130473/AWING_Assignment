using MediatR;
using PirateTreasure.Application.Contracts.Persistence;

namespace PirateTreasure.Application.Features.TreasureMaps.Commands.SolveTreasureMap
{
    public class SolveTreasureMapCommandHandler : IRequestHandler<SolveTreasureMapCommand, double>
    {
        private readonly ITreasureMapRepository _treasureMapRepository;

        public SolveTreasureMapCommandHandler(ITreasureMapRepository treasureMapRepository)
        {
            _treasureMapRepository = treasureMapRepository;
        }

        public async Task<double> Handle(SolveTreasureMapCommand request, CancellationToken cancellationToken)
        {
            var map = await _treasureMapRepository.GetByIdAsync(request.TreasureMapId);
            if (map == null || map.Cells.Count == 0)
                throw new InvalidOperationException("Bản đồ không tồn tại hoặc không có dữ liệu.");

            int p = map.MaxChestValue;

            var chestValues = map.Cells.Select(c => c.ChestValue).ToList();

            if (chestValues.Any(val => val < 1 || val > p))
                throw new InvalidOperationException($"Rương chỉ được phép có giá trị từ 1 đến {p}.");

            var requiredValues = Enumerable.Range(1, p);
            var existingValues = chestValues.ToHashSet();
            var missing = requiredValues.Where(v => !existingValues.Contains(v)).ToList();

            if (missing.Any())
                throw new InvalidOperationException($"Thiếu các rương: {string.Join(", ", missing)}.");

            var startCell = map.Cells.FirstOrDefault(c => c.Row == 1 && c.Col == 1);
            if (startCell == null || startCell.ChestValue != 1)
                throw new InvalidOperationException("Ô (1,1) phải chứa rương số 1 để bắt đầu hành trình.");

            // Gom nhóm các vị trí theo số rương
            var chestGroups = new Dictionary<int, List<(int row, int col)>>();
            foreach (var cell in map.Cells)
            {
                if (!chestGroups.ContainsKey(cell.ChestValue))
                    chestGroups[cell.ChestValue] = new List<(int, int)>();

                chestGroups[cell.ChestValue].Add((cell.Row - 1, cell.Col - 1));
            }

            var distance = new double[map.Rows, map.Columns];
            for (int i = 0; i < map.Rows; i++)
                for (int j = 0; j < map.Columns; j++)
                    distance[i, j] = double.MaxValue;

            distance[0, 0] = 0;

            var queue = new List<(int row, int col, double cost)> { (0, 0, 0) };

            foreach (var target in chestGroups[1])
            {
                double min = double.MaxValue;
                foreach (var source in queue)
                {
                    double cost = GetDistance(source.row, source.col, target.row, target.col) + source.cost;
                    min = Math.Min(min, cost);
                }

                distance[target.row, target.col] = min;
            }

            for (int value = 2; value <= p; value++)
            {
                var prevPositions = chestGroups[value - 1];
                var currPositions = chestGroups[value];

                var temp = new double[map.Rows, map.Columns];
                for (int i = 0; i < map.Rows; i++)
                    for (int j = 0; j < map.Columns; j++)
                        temp[i, j] = double.MaxValue;

                foreach (var target in currPositions)
                {
                    double min = double.MaxValue;

                    foreach (var source in prevPositions)
                    {
                        double cost = GetDistance(source.row, source.col, target.row, target.col) + distance[source.row, source.col];
                        min = Math.Min(min, cost);
                    }

                    temp[target.row, target.col] = min;
                }

                distance = temp;
            }

            var lastChest = chestGroups[p];
            double result = lastChest.Min(c => distance[c.row, c.col]);

            return Math.Round(result, 6);
        }

        private double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}