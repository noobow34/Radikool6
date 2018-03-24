import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Program} from '../../interfaces/program';
import {Reserve} from '../../interfaces/reserve';
import {ReserveService} from '../../services/reserve.service';

@Component({
  selector: 'app-reserve-edit',
  templateUrl: './reserve-edit.component.html',
  styleUrls: ['./reserve-edit.component.scss']
})
export class ReserveEditComponent implements OnInit {

  constructor( public dialogRef: MatDialogRef<ReserveEditComponent>,
               @Inject(MAT_DIALOG_DATA) public data:{program?: Program, reserve?: Reserve},
               private reserveService: ReserveService) {

    console.log(data.program);
    this.reserveService.update({ stationId: data.program.stationId, start: data.program.start, end: data.program.end }).subscribe(res =>{
      console.log(res);
    });

  }

  ngOnInit() {
  }

}
