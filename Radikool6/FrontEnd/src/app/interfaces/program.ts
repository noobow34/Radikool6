export interface Program {
  cast: string;
  description: string;
  end: Date;
  id: string;
  start: Date;
  stationId: string;
  title: string;
  reservable?: boolean;
}

export interface ProgramSearchCondition {
  stationId: string;
  from?: string;
  to?: string;
  keyword?: string;
  refresh?: boolean;
}
