import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Program} from '../program';
import {Reserve} from '../reserve';

@Component({
  selector: 'app-reserve-edit',
  templateUrl: './reserve-edit.component.html',
  styleUrls: ['./reserve-edit.component.scss']
})
export class ReserveEditComponent implements OnInit {

  constructor( public dialogRef: MatDialogRef<ReserveEditComponent>,
               @Inject(MAT_DIALOG_DATA) public data:{program?: Program, reserve?: Reserve}) {

    console.log(data.program)
  }

  ngOnInit() {
  }

}
