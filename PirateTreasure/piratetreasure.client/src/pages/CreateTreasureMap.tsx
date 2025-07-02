import React, { useState } from "react";
import {
  Box,
  Button,
  Grid,
  TextField,
  Typography,
  Container,
  Paper,
  Alert,
  Snackbar,
} from "@mui/material";
import { createTreasureMap } from "../api/treasureMapApi";

const CreateTreasureMap: React.FC = () => {
  const [name, setName] = useState("");
  const [rows, setRows] = useState(3);
  const [cols, setCols] = useState(3);
  const [maxChestValue, setMaxChestValue] = useState(5);
  const [matrix, setMatrix] = useState<number[][]>(
    Array(3).fill(Array(3).fill(1))
  );
  const [success, setSuccess] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleGenerateMatrix = () => {
    const newMatrix = Array(rows)
      .fill(0)
      .map(() => Array(cols).fill(1));
    setMatrix(newMatrix);
  };

  const handleCellChange = (rowIdx: number, colIdx: number, value: number) => {
    const updated = [...matrix];
    updated[rowIdx] = [...updated[rowIdx]];
    updated[rowIdx][colIdx] = value;
    setMatrix(updated);
  };

  const handleSubmit = async () => {
    // Validate cơ bản
    if (!name.trim()) {
      setError("Tên bản đồ không được để trống.");
      return;
    }

    if (rows <= 0 || cols <= 0 || maxChestValue <= 0) {
      setError("Số hàng, số cột và p phải lớn hơn 0.");
      return;
    }

    if (maxChestValue > rows * cols) {
      setError("Giá trị p không được lớn hơn tổng số ô.");
      return;
    }

    if (matrix.length !== rows || matrix.some((r) => r.length !== cols)) {
      setError("Số ô không khớp với kích thước bản đồ.");
      return;
    }

    // Validate giá trị các ô
    const flatCells = matrix.flat().map((v) => parseInt(`${v}`, 10));
    if (flatCells.some((v) => isNaN(v) || v < 1 || v > maxChestValue)) {
      setError(
        `Tất cả rương phải có giá trị trong khoảng 1 đến ${maxChestValue}.`
      );
      return;
    }

    // Validate phải có đủ rương từ 1 đến p
    const chestSet = new Set(flatCells);
    const missing: number[] = [];
    for (let i = 1; i <= maxChestValue; i++) {
      if (!chestSet.has(i)) missing.push(i);
    }

    if (missing.length > 0) {
      setError(`Thiếu các rương số: ${missing.join(", ")}.`);
      return;
    }

    // Ô (1,1) phải là 1
    if (matrix[0][0] !== 1) {
      setError("Ô (1,1) phải chứa rương số 1 để bắt đầu hành trình.");
      return;
    }

    // OK - submit
    try {
      const cells = matrix.flatMap((row, rIdx) =>
        row.map((val, cIdx) => ({
          row: rIdx + 1,
          col: cIdx + 1,
          chestValue: val,
        }))
      );

      const data = {
        name,
        rows,
        columns: cols,
        maxChestValue,
        cells,
      };

      const id = await createTreasureMap(data);
      setSuccess(`Bản đồ đã lưu với ID: ${id}`);
      setError(null);
    } catch {
      setError("Lỗi khi tạo bản đồ.");
    }
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h4" gutterBottom>
        Tạo bản đồ kho báu
      </Typography>

      <Paper sx={{ p: 3, mb: 4 }}>
        <Box display="flex" flexDirection="column" gap={2}>
          <TextField
            label="Tên bản đồ"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <Box display="flex" gap={2}>
            <TextField
              label="Số hàng"
              type="number"
              value={rows}
              inputProps={{ min: 1, max: 500 }}
              onChange={(e) =>
                setRows(Math.min(500, Math.max(1, +e.target.value)))
              }
            />
            <TextField
              label="Số cột"
              type="number"
              value={cols}
              inputProps={{ min: 1, max: 500 }}
              onChange={(e) =>
                setCols(Math.min(500, Math.max(1, +e.target.value)))
              }
            />
            <TextField
              label="Số rương lớn nhất (p)"
              type="number"
              value={maxChestValue}
              inputProps={{ min: 1 }}
              onChange={(e) => setMaxChestValue(Math.max(1, +e.target.value))}
            />
            <Button variant="contained" onClick={handleGenerateMatrix}>
              Tạo ma trận
            </Button>
          </Box>

          <Typography variant="h6">Nhập giá trị rương cho từng ô:</Typography>

          <Box sx={{ overflowX: "auto" }}>
            <Grid container spacing={1}>
              {matrix.map((row, rowIdx) => (
                <Grid key={rowIdx} container item spacing={1}>
                  {row.map((val, colIdx) => (
                    <Grid item key={colIdx}>
                      <TextField
                        type="number"
                        value={val}
                        onChange={(e) =>
                          handleCellChange(
                            rowIdx,
                            colIdx,
                            parseInt(e.target.value) || 1
                          )
                        }
                        inputProps={{
                          min: 1,
                          max: maxChestValue,
                          style: { width: 60, textAlign: "center" },
                        }}
                      />
                    </Grid>
                  ))}
                </Grid>
              ))}
            </Grid>
          </Box>

          <Button
            variant="contained"
            color="primary"
            onClick={handleSubmit}
            sx={{ mt: 2 }}
          >
            Lưu bản đồ
          </Button>
        </Box>
      </Paper>

      {/* Snackbar thông báo */}
      <Snackbar
        open={!!success}
        autoHideDuration={6000}
        onClose={() => setSuccess(null)}
      >
        <Alert severity="success" onClose={() => setSuccess(null)}>
          {success}
        </Alert>
      </Snackbar>

      <Snackbar
        open={!!error}
        autoHideDuration={6000}
        onClose={() => setError(null)}
      >
        <Alert severity="error" onClose={() => setError(null)}>
          {error}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default CreateTreasureMap;
