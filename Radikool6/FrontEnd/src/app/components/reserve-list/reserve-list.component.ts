import { Component, OnInit } from '@angular/core';
import {ReserveService} from '../../services/reserve.service';
import {Reserve} from '../../interfaces/reserve';
import {ReserveEditComponent} from '../reserve-edit/reserve-edit.component';
import {MatDialog} from "@angular/material";

@Component({
  selector: 'app-reserve-list',
  templateUrl: './reserve-list.component.html',
  styleUrls: ['./reserve-list.component.scss']
})
export class ReserveListComponent implements OnInit {

  public reserves: Reserve[] = [];

  constructor(
    private reserveService: ReserveService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.reserveService.get().subscribe(res => {
      if (res.result){
        this.reserves = res.data;
      }
    });
  }

  /**
   * 予約編集
   * @param {Reserve} reserve
   */
  public editReserve = (reserve: Reserve) => {
    console.log(reserve);



  }

}
