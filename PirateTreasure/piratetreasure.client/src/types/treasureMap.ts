export interface TreasureCellDto {
  row: number;
  col: number;
  chestValue: number;
}

export interface TreasureMapDto {
  id: string;
  name: string;
  rows: number;
  columns: number;
  maxChestValue: number;
  cells: TreasureCellDto[];
}
