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

            // Kiểm tra hợp lệ giá trị rương
            var chestValues = map.Cells.Select(c => c.ChestValue).ToList();

            if (chestValues.Any(val => val < 1 || val > p))
                throw new InvalidOperationException($"Rương chỉ được phép có giá trị từ 1 đến {p}.");

            var requiredValues = Enumerable.Range(1, p);
            var existingValues = chestValues.ToHashSet();
            var missing = requiredValues.Where(v => !existingValues.Contains(v)).ToList();

            if (missing.Any())
                throw new InvalidOperationException($"Thiếu các rương: {string.Join(", ", missing)}.");

            // Tìm ô bắt đầu (1,1) chứa rương số 1
            var startCell = map.Cells.FirstOrDefault(c => c.Row == 1 && c.Col == 1);
            if (startCell == null || startCell.ChestValue != 1)
                throw new InvalidOperationException("Ô (1,1) phải chứa rương số 1 để bắt đầu hành trình.");

            // Gom nhóm các vị trí theo số rương
            var chestGroups = new Dictionary<int, List<(int row, int col)>>();
            foreach (var cell in map.Cells)
            {
                if (!chestGroups.ContainsKey(cell.ChestValue))
                    chestGroups[cell.ChestValue] = new List<(int, int)>();

                // Chuyển về chỉ số mảng 0-based
                chestGroups[cell.ChestValue].Add((cell.Row - 1, cell.Col - 1));
            }

            // Khởi tạo mảng lưu nhiên liệu
            var distance = new double[map.Rows, map.Columns];
            for (int i = 0; i < map.Rows; i++)
                for (int j = 0; j < map.Columns; j++)
                    distance[i, j] = double.MaxValue;

            // Gán điểm bắt đầu có nhiên liệu 0 --> [1,1] là trên giao diện UI, [0,0] là trên code ma trận
            var startRow = startCell.Row - 1;
            var startCol = startCell.Col - 1;
            distance[startRow, startCol] = 0;

            // 🔁 Duyệt qua từng bậc từ rương 1 → 2 → 3 → ... → p
            for (int value = 2; value <= p; value++)
            {
                // ✅ Lấy danh sách vị trí chứa rương value - 1 (nguồn) và value (đích)
                var prevPositions = chestGroups[value - 1];
                var currPositions = chestGroups[value];

                // 🧪 Tạo ma trận tạm để lưu lượng nhiên liệu ít nhất đến từng ô chứa rương `value`
                var temp = new double[map.Rows, map.Columns];
                for (int i = 0; i < map.Rows; i++)
                    for (int j = 0; j < map.Columns; j++)
                        temp[i, j] = double.MaxValue; // Khởi tạo toàn bộ bằng ∞ (chưa thể đến)

                // 📦 Với mỗi vị trí có rương `value` (đích), ta xét tất cả các vị trí rương `value - 1` (nguồn)
                foreach (var target in currPositions)
                {
                    double min = double.MaxValue; // Biến tạm để giữ lượng nhiên liệu nhỏ nhất đến `target`

                    foreach (var source in prevPositions)
                    {
                        // 🔢 Tính tổng nhiên liệu để đi từ rương 1 → ... → value - 1 → value
                        double cost =
                            GetDistance(source.row, source.col, target.row, target.col) // fuel từ rương value - 1 → value
                            + distance[source.row, source.col];                         // fuel đã tích lũy từ rương 1 → value - 1

                        // 🔍 So sánh và chọn đường đi ít nhiên liệu nhất
                        min = Math.Min(min, cost);
                    }

                    // ✅ Lưu lại lượng nhiên liệu tốt nhất để đi đến ô rương `value` hiện tại
                    temp[target.row, target.col] = min;
                }

                // 🔄 Cập nhật lại `distance` cho vòng lặp tiếp theo (value + 1)
                distance = temp;
            }


            // Tìm ô chứa rương p có chi phí nhỏ nhất
            var lastChests = chestGroups[p];
            double result = lastChests.Min(c => distance[c.row, c.col]);

            return Math.Round(result, 6);
        }

        // Tính khoảng cách giữa 2 ô (hệ tọa độ mảng 0-based)
        private double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

    }
}