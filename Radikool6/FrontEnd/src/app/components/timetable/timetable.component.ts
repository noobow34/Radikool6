import {Component, OnDestroy, OnInit} from '@angular/core';
import {StationService} from '../../services/station.service';
import {Station} from '../../interfaces/station';
import {ProgramService} from '../../services/program.service';
import moment = require('moment');
import {Program, ProgramSearchCondition} from '../../interfaces/program';
import {StateService} from '../../services/state.service';
import {Moment} from 'moment';
import {MatTableDataSource} from "@angular/material";
import {Observable} from "rxjs/Rx";
import {ReserveEditComponent} from "../reserve-edit/reserve-edit.component";

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.scss']
})
export class TimetableComponent implements OnInit {

  public radiko = {};
  public radikoRegions = [];
  public date = moment().format('YYYY-MM-DD');
  public stationId;

  public programs: Program[] = [];
  public days: Moment[] = [];

  public loadingStation = false;
  public loadingProgram = false;


  constructor(private stationService: StationService,
              private programService: ProgramService,
              private stateService: StateService) {
  }

  ngOnInit() {
    this.stationService.get('radiko').subscribe(res => {
      // 種別、地域ごとに分類する
      const stations = res.data as Station[];
      this.radiko = {};
      this.radikoRegions = [];

      const nhk = {};
      for (const station of stations) {
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


  public onClickRefresh = () => {
    this.stationService.refresh('radiko').subscribe(res => {
      console.log(res);
    });
  }

  /**
   * 番組表表示
   * @param {Station} station
   */
  public setStation = (station: Station) => {
    this.stationId = station.id;
    const condition: ProgramSearchCondition = {
      stationId: station.id,
      from: `${this.date} 05:00:00`,
      to: moment(this.date).add(1, 'days').format('YYYY-MM-DD 04:59:59'),
      refresh: true
    };
    this.search(condition);

  }

  public setDate = () => {
    const condition: ProgramSearchCondition = {
      stationId: this.stationId,
      from: `${this.date} 05:00:00`,
      to: moment(this.date).add(1, 'days').format('YYYY-MM-DD 04:59:59'),
      refresh: true
    };
    this.search(condition);
  }

  private search = (condition: ProgramSearchCondition) => {
    this.loadingProgram = true;
    this.programService.search(condition).subscribe(res => {
      if (res.result) {
        this.programs = res.data.programs;

        const now = moment();
        this.programs.forEach(p => {
          p.reservable = moment(p.end) >= moment();
        });

        const min = moment(res.data.range[0]);
        const max = moment(res.data.range[1]);
        this.days = [];
        while (min < max) {
          this.days.push(moment(min));

          min.add(1, 'days');
        }
      }
      this.loadingProgram = false;
    });
  }


  /**
   * 番組詳細表示
   * @param type
   * @param {Program} program
   */
  public editReserve = (type: string, program: Program) => {
    this.stateService.editReserve({program: program}, () => {

    });

  }

  /**
   * タイムフリー
   * @param {Program} program
   */
  public getTimeFree = (program: Program) => {
    this.programService.getTimeFree(program.id).subscribe(res => {
      if (res.result) {
        this.stateService.showTimefreeProgress(() => {

        });
      }
    });
  }


}
