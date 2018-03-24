import { Component, OnInit } from '@angular/core';
import {ReserveService} from "../reserve.service";
import {Reserve} from "../reserve";

@Component({
  selector: 'app-reserve-list',
  templateUrl: './reserve-list.component.html',
  styleUrls: ['./reserve-list.component.scss']
})
export class ReserveListComponent implements OnInit {

  public reserves: Reserve[] = [];

  constructor(private reserveService: ReserveService) { }

  ngOnInit() {
    this.reserveService.get().subscribe(res => {
      if (res.result){
        this.reserves = res.data;
      }
    });
  }

}
