import { Component, OnInit } from '@angular/core';
import {StationService} from '../../services/station.service';
import {Station} from '../../interfaces/station';
import {ProgramService} from '../../services/program.service';
import moment = require('moment');
import {Program, ProgramSearchCondition} from '../../interfaces/program';
import {MatDialog} from '@angular/material';
import {ReserveEditComponent} from "../reserve-edit/reserve-edit.component";

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.scss']
})
export class TimetableComponent implements OnInit {

  public radiko = {};
  public radikoRegions = [];
  public date = moment();
  public programs:Program[] = [];

  public loadingStation = false;
  public loadingProgram = false;

  constructor(private stationService: StationService,
              private programService: ProgramService,
              public dialog: MatDialog) {
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
    const cond: ProgramSearchCondition = {
      stationId: station.id,
      from: this.date.format('YYYY-MM-DD 05:00:00'),
      to:  this.date.clone().add(1, 'days').format('YYYY-MM-DD 04:59:59'),
      refresh: true
    };
    this.loadingProgram = true;
    this.programService.search(cond).subscribe(res => {
      if (res.result){
        this.programs = res.data as Program[];
        console.log(this.programs);
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
    let dialogRef = this.dialog.open(ReserveEditComponent, {
      width: '250px',
      data: { program: program }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });


    console.log(program);
  }


}
