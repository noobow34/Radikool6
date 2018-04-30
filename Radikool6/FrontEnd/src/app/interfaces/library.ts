import {Program} from './program';

export interface Library {
  id?: string;
  fileName?: string;
  path?: string;
  program?: Program;
}
