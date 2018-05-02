export interface Config {
  saveDir?: string;
  radikoEmail?: string;
  radikoPassword?: string;

  fileName?: string;
  timeFreeMargin?: number;

  tagTitle?: string;
  tagArtist?: string;
  tagAlbum?: string;
  tagGenre?: string;
  tagComment?: string;

  samplingRate?: string;
  bitRate?: string;
  volume?: string;
}
