import React, { useEffect, useState } from "react";
import {
  Container,
  Typography,
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Button,
  CircularProgress,
  Snackbar,
  Alert,
} from "@mui/material";
import { getAllTreasureMaps, solveTreasureMap } from "../api/treasureMapApi";
import type { TreasureMapDto } from "../types/treasureMap";

const TreasureMapList: React.FC = () => {
  const [maps, setMaps] = useState<TreasureMapDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [solvingId, setSolvingId] = useState<string | null>(null);
  const [result, setResult] = useState<{ id: string; fuel: number } | null>(
    null
  );
  const [error, setError] = useState<string | null>(null);

  const fetchMaps = async () => {
    setLoading(true);
    try {
      const data = await getAllTreasureMaps();
      setMaps(data);
    } catch (err) {
      setError("Không thể tải danh sách bản đồ.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMaps();
  }, []);

  const handleSolve = async (id: string) => {
    try {
      setSolvingId(id);
      const fuel = await solveTreasureMap(id);
      setResult({ id, fuel });
    } catch {
      setError("Lỗi khi giải bản đồ.");
    } finally {
      setSolvingId(null);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Danh sách bản đồ kho báu
      </Typography>

      <Paper elevation={3}>
        {loading ? (
          <CircularProgress sx={{ m: 4 }} />
        ) : (
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Tên bản đồ</TableCell>
                <TableCell>Số hàng</TableCell>
                <TableCell>Số cột</TableCell>
                <TableCell>Số rương lớn nhất (p)</TableCell>
                <TableCell>Hành động</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {maps.map((map) => (
                <TableRow key={map.id}>
                  <TableCell>{map.name}</TableCell>
                  <TableCell>{map.rows}</TableCell>
                  <TableCell>{map.columns}</TableCell>
                  <TableCell>{map.maxChestValue}</TableCell>
                  <TableCell>
                    <Button
                      variant="contained"
                      color="primary"
                      disabled={solvingId === map.id}
                      onClick={() => handleSolve(map.id)}
                    >
                      {solvingId === map.id ? "Đang giải..." : "Giải bản đồ"}
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </Paper>

      {/* Kết quả giải */}
      <Snackbar
        open={!!result}
        autoHideDuration={6000}
        onClose={() => setResult(null)}
      >
        <Alert severity="success" onClose={() => setResult(null)}>
          🔑 Đã tìm được kho báu! Nhiên liệu tối thiểu:{" "}
          {result?.fuel.toFixed(6)}
        </Alert>
      </Snackbar>

      {/* Lỗi */}
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

export default TreasureMapList;
