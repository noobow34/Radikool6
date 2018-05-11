export interface Station {
  id: string;
  regionId: string;
  regionName: string;
  area: string;
  type: string;
  code: string;
  name: string;
  sequence: string;
  mediaUrl: string;
  timetableUrl: string;

  checked: boolean;
}
