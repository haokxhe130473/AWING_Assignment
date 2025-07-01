import axios from "axios";
import type { TreasureMapDto } from "../types/treasureMap";

const API_BASE = `${import.meta.env.VITE_API_BASE_URL}/api/treasure-maps`;

export const createTreasureMap = async (data: {
  name: string;
  rows: number;
  columns: number;
  maxChestValue: number;
  cells: { row: number; col: number; chestValue: number }[];
}) => {
  const res = await axios.post(`${API_BASE}`, data);
  return res.data;
};

export const getAllTreasureMaps = async (): Promise<TreasureMapDto[]> => {
  const res = await axios.get(`${API_BASE}`);
  return res.data;
};

export const getTreasureMapById = async (
  id: string
): Promise<TreasureMapDto> => {
  const res = await axios.get(`${API_BASE}/${id}`);
  return res.data;
};

export const solveTreasureMap = async (id: string): Promise<number> => {
  const res = await axios.post(`${API_BASE}/${id}/solve`);
  return res.data;
};
