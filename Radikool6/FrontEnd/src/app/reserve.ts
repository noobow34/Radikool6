export interface Reserve {
  id?: string;
  stationId?: string;
  start?: Date;
  end?: Date;
  formatId?: string;
  repeat?: string[];
  enabled?: boolean;
}
