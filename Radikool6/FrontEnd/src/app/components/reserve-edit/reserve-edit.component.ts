import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Program} from '../../interfaces/program';
import {Reserve} from '../../interfaces/reserve';
import {ReserveService} from '../../services/reserve.service';
import {StationService} from '../../services/station.service';
import {Station} from '../../interfaces/station';
import moment = require("moment");

@Component({
  selector: 'app-reserve-edit',
  templateUrl: './reserve-edit.component.html',
  styleUrls: ['./reserve-edit.component.scss']
})
export class ReserveEditComponent implements OnInit {
  public reserve: Reserve = {};
  public stations: Station[] = [];
  public hours: number[] = [];
  public minutes: number[] = [];

  public startDate;
  public startHour;
  public startMinute;
  public endDate;
  public endHour;
  public endMinute;


  constructor(
    public dialogRef: MatDialogRef<ReserveEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{program?: Program, reserve?: Reserve},
    private reserveService: ReserveService,
    private stationService: StationService) {

    console.log(data.program);

    if(data.program){
      this.reserve = {
        name: this.data.program.title,
        stationId: data.program.stationId,
        start: data.program.start,
        end: data.program.end
      };

    } else {
      this.reserve = Object.assign({}, data.reserve);
    }

    const start = moment(this.reserve.start);
    const end = moment(this.reserve.end);

    this.startDate = start.toDate();
    this.startHour = start.hour();
    this.startMinute = start.minute();

    this.endDate = end.toDate();
    this.endHour = end.hour();
    this.endMinute = end.minute();

  }

  ngOnInit() {
    for(let i=0 ; i<24 ; i++){
      this.hours.push(i);
    }
    for(let i=0 ; i<60 ; i++){
      this.minutes.push(i);
    }

    this.stationService.get().subscribe(res => {
      if(res.result){
        this.stations = res.data;
      }
    });
  }


  /**
   * 削除
   */
  public delete = () => {
    this.reserveService.delete(this.reserve.id).subscribe(res =>{
      if(res.result) {
        this.dialogRef.close(true);
      }
    });
  }

  /**
   * 保存
   */
  public save = () => {
    this.reserve.start = moment(this.startDate).hour(this.startHour).minute(this.startMinute).toDate();
    this.reserve.end = moment(this.endDate).hour(this.endHour).minute(this.endMinute).toDate();

    this.reserveService.update(this.reserve).subscribe(res =>{
      if(res.result) {
        this.dialogRef.close(true);
      }
    });
  }

}
