export interface Reserve {
  id?: string;
  name?: string;
  stationId?: string;
  stationName?: string;
  start?: Date;
  end?: Date;
  formatId?: string;
  repeat?: string[];
  enabled?: boolean;
}
