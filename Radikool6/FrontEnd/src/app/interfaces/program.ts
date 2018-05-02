export interface Program {
  cast: string;
  description: string;
  end: string;
  id: string;
  start: string;
  stationId: string;
  title: string;
  reservable?: boolean;
  tsNg?: string;
}

export interface ProgramSearchCondition {
  stationId: string;
  from?: string;
  to?: string;
  keyword?: string;
  refresh?: boolean;
}
