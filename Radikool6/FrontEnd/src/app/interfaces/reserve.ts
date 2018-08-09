export interface Reserve {
  id?: string;
  name?: string;
  stationId?: string;
  stationName?: string;
  start?: string;
  end?: string;
  formatId?: string;
  repeat?: number[];
  enabled?: boolean;
  isTimeFree?: boolean;
}
