import { Component, OnInit } from '@angular/core';
import {StationService} from '../station.service';
import {Station} from '../station';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.scss']
})
export class TimetableComponent implements OnInit {

  public radiko = {};
  public radikoRegions = [];

  constructor(private stationService: StationService) {
  }

  ngOnInit() {
    this.stationService.get().subscribe(res => {
      // 種別、地域ごとに分類する
      let stations = res.data as Station[];
      this.radiko = {};
      this.radikoRegions = [];

      let nhk = {};
      for (const station of stations) {
        if (station.type === 'radiko') {
          if (!(station.regionName in this.radiko)) {
            this.radiko[station.regionName] = [];
            this.radikoRegions.push(station.regionName);
          }
          this.radiko[station.regionName].push(station);
        }
      }

      console.log(this.radikoRegions);
    });
  }

  /**
   * 番組表表示
   * @param {Station} station
   */
  public setStation = (station: Station) => {
    console.log(station);
  }
}
