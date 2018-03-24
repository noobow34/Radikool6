import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Program} from '../../interfaces/program';
import {Reserve} from '../../interfaces/reserve';
import {ReserveService} from '../../services/reserve.service';
import {StationService} from '../../services/station.service';
import {Station} from '../../interfaces/station';

@Component({
  selector: 'app-reserve-edit',
  templateUrl: './reserve-edit.component.html',
  styleUrls: ['./reserve-edit.component.scss']
})
export class ReserveEditComponent implements OnInit {
  public reserve: Reserve = {};
  public stations: Station[] = [];


  constructor(
    public dialogRef: MatDialogRef<ReserveEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{program?: Program, reserve?: Reserve},
    private reserveService: ReserveService,
    private stationService: StationService) {

    console.log(data.program);

    if(data.program){
      this.reserve = {
        stationId: data.program.stationId,
        start: data.program.start,
        end: data.program.end
      };

    } else {
      this.reserve = Object.assign({}, data.reserve);
    }

    console.log(this.reserve);
/*
    this.reserveService.update({ stationId: data.program.stationId, start: data.program.start, end: data.program.end }).subscribe(res =>{
      console.log(res);
    });
*/
  }

  ngOnInit() {
    this.stationService.get().subscribe(res => {
      if(res.result){
        this.stations = res.data;
      }
    });
  }

  public save = () => {
    this.reserveService.update(this.reserve).subscribe(res =>{
      if(res.result) {
        this.dialogRef.close(true);
      }
    });
  }

}
