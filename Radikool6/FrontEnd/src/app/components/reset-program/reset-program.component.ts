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
  public listenRadio = {};
  public listenRadioRegions = [];

  public total = 0;
  public progress = 0;
  public loading = false;

  private stations: {[key: string]: Station[]} = {};

  constructor(private stationService: StationService,
              private programService: ProgramService) {
  }

  ngOnInit() {
    this.stationService.get('radiko').subscribe(res => {
      // 種別、地域ごとに分類する
      this.stations['radiko'] = res.data;
      this.radiko = {};
      this.radikoRegions = [];

      for (const station of this.stations['radiko']) {
        if (station.type === 'radiko') {
          if (!(station.regionName in this.radiko)) {
            this.radiko[station.regionName] = [];
            this.radikoRegions.push(station.regionName);
          }
          this.radiko[station.regionName].push(station);
        }
      }
    });

    this.stationService.get('lr').subscribe(res => {
      // 種別、地域ごとに分類する
      this.stations['lr'] = res.data;
      this.listenRadio = {};
      this.listenRadioRegions= [];

      for (const station of this.stations['lr']) {
        if (station.type === 'lr') {
          if (!(station.regionName in this.listenRadio)) {
            this.listenRadio[station.regionName] = [];
            this.listenRadioRegions.push(station.regionName);
          }
          this.listenRadio[station.regionName].push(station);
        }
      }
    });

  }

  /**
   * 地域全チェック／解除
   * @param {MatCheckboxChange} e
   * @param stations
   */
  public toggleCheck = (e: MatCheckboxChange, stations) => {
    stations.forEach(s => {
      s.checked = e.checked;
    });
  }

  public submit = () => {
    this.loading = true;
    const stationIds = this.stations['radiko'].filter(s => s.checked).map(s => s.id)
      .concat(this.stations['lr'].filter(s => s.checked).map(s => s.id));

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
