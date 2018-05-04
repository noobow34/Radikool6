import {Program} from './program';

export interface Library {
  id?: string;
  fileName?: string;
  path?: string;
  program?: Program;
  size?: string;
  created?: Date;
}
