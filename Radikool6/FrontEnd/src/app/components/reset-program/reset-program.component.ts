import { Component, OnInit } from '@angular/core';
import {ProgramService} from '../../services/program.service';
import {Station} from '../../interfaces/station';
import {StationService} from '../../services/station.service';
import {MatCheckboxChange} from '@angular/material';

@Component({
  selector: 'app-reset-program',
  templateUrl: './reset-program.component.html',
  styleUrls: ['./reset-program.component.scss']
})
export class ResetProgramComponent implements OnInit {

  public radiko = {};
  public radikoRegions = [];
  public total = 0;
  public progress = 0;
  public loading = false;

  private stations: Station[] = [];

  constructor(private stationService: StationService,
              private programService: ProgramService) {
  }

  ngOnInit() {
    this.stationService.get().subscribe(res => {
      // 種別、地域ごとに分類する
      this.stations = res.data;
      this.radiko = {};
      this.radikoRegions = [];

      let nhk = {};
      for (const station of this.stations) {
        if (station.type === 'radiko') {
          if (!(station.regionName in this.radiko)) {
            this.radiko[station.regionName] = [];
            this.radikoRegions.push(station.regionName);
          }
          this.radiko[station.regionName].push(station);
        }
      }
    });
  }

  /**
   * 地域全チェック／解除
   * @param {MatCheckboxChange} e
   * @param region
   */
  public toggleCheck = (e: MatCheckboxChange, region) => {
    this.radiko[region].forEach(s => {
      s.checked = e.checked;
    });
  }

  public submit = () => {
    this.loading = true;
    const stationIds = this.stations.filter(s => s.checked).map(s => s.id);
    this.total = stationIds.length;
    this.progress = 1;
    let i = 0;
    const refresh = () => {
      this.programService.refresh(stationIds[i]).subscribe(res => {
        if (res.result) {
          i++;
          this.progress++;
          if (i < stationIds.length) {
            refresh();
          } else {
            this.loading = false;
          }
        }
      });
    }

    refresh();

  }

}
