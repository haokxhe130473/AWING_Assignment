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
} from "@mui/material";

import axios from "axios";

const CreateTreasureMap: React.FC = () => {
  const [name, setName] = useState("");
  const [rows, setRows] = useState(3);
  const [cols, setCols] = useState(3);
  const [maxChestValue, setMaxChestValue] = useState(5);
  const [matrix, setMatrix] = useState<number[][]>(
    Array(3).fill(Array(3).fill(1))
  );
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  // Cập nhật số hàng/cột và làm lại ma trận
  const handleSizeChange = (newRows: number, newCols: number) => {
    const newMatrix: number[][] = [];
    for (let i = 0; i < newRows; i++) {
      newMatrix[i] = [];
      for (let j = 0; j < newCols; j++) {
        newMatrix[i][j] = matrix[i]?.[j] ?? 1;
      }
    }
    setMatrix(newMatrix);
  };

  const handleCellChange = (row: number, col: number, value: number) => {
    const newMatrix = [...matrix];
    newMatrix[row][col] = value;
    setMatrix(newMatrix);
  };

  const validateMatrix = () => {
    const flat = matrix.flat();
    const allInRange = flat.every((v) => v >= 1 && v <= maxChestValue);
    const containsP = flat.includes(maxChestValue);
    return allInRange && containsP;
  };

  const handleSubmit = async () => {
    setError("");
    setSuccessMessage("");

    if (!validateMatrix()) {
      setError(
        `Mỗi ô phải từ 1 đến ${maxChestValue} và phải chứa ít nhất 1 ô có giá trị ${maxChestValue}.`
      );
      return;
    }

    const cells = matrix.flatMap((rowArr, rowIdx) =>
      rowArr.map((val, colIdx) => ({
        row: rowIdx + 1,
        col: colIdx + 1,
        chestValue: val,
      }))
    );

    const payload = {
      name,
      rows,
      columns: cols,
      maxChestValue,
      cells,
    };

    try {
      const res = await axios.post("/api/treasure-maps", payload);
      setSuccessMessage(`Tạo bản đồ thành công! ID: ${res.data}`);
    } catch (err) {
      setError("Tạo bản đồ thất bại. Vui lòng kiểm tra lại.");
    }
  };

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Tạo bản đồ kho báu
        </Typography>

        <Box display="flex" gap={2} mb={2}>
          <TextField
            label="Tên bản đồ"
            value={name}
            onChange={(e) => setName(e.target.value)}
            fullWidth
          />
          <TextField
            label="Số hàng (n)"
            type="number"
            value={rows}
            onChange={(e) => {
              const newVal = parseInt(e.target.value) || 1;
              setRows(newVal);
              handleSizeChange(newVal, cols);
            }}
            sx={{ width: 150 }}
            inputProps={{ min: 1, max: 500 }}
          />
          <TextField
            label="Số cột (m)"
            type="number"
            value={cols}
            onChange={(e) => {
              const newVal = parseInt(e.target.value) || 1;
              setCols(newVal);
              handleSizeChange(rows, newVal);
            }}
            sx={{ width: 150 }}
            inputProps={{ min: 1, max: 500 }}
          />
          <TextField
            label="Giá trị rương lớn nhất (p)"
            type="number"
            value={maxChestValue}
            onChange={(e) => setMaxChestValue(parseInt(e.target.value) || 1)}
            sx={{ width: 180 }}
            inputProps={{ min: 1 }}
          />
        </Box>

        <Box mt={2}>
          <Typography variant="h6" gutterBottom>
            Nhập ma trận rương ({rows} x {cols})
          </Typography>
          <Grid container spacing={1}>
            {matrix.map((row, rowIdx) => (
              <Grid container key={rowIdx} spacing={1}>
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

        <Box mt={3}>
          <Button variant="contained" onClick={handleSubmit}>
            Lưu bản đồ
          </Button>
        </Box>

        {error && (
          <Alert severity="error" sx={{ mt: 2 }}>
            {error}
          </Alert>
        )}
        {successMessage && (
          <Alert severity="success" sx={{ mt: 2 }}>
            {successMessage}
          </Alert>
        )}
      </Paper>
    </Container>
  );
};

export default CreateTreasureMap;
