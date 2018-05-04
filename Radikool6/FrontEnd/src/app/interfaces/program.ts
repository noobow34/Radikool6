import {Station} from './station';
import {SafeHtml} from '@angular/platform-browser';

export interface Program {
  cast: string;
  description: string;
  descriptionHTML: SafeHtml;
  end: string;
  id: string;
  start: string;
  stationId: string;
  title: string;
  reservable?: boolean;
  tsNg?: string;
  station?: Station;
}

export interface ProgramSearchCondition {
  stationId: string;
  from?: string;
  to?: string;
  keyword?: string;
  refresh?: boolean;
}
