import { Component, OnInit } from '@angular/core';
import {ReserveService} from "../reserve.service";

@Component({
  selector: 'app-reserve-list',
  templateUrl: './reserve-list.component.html',
  styleUrls: ['./reserve-list.component.scss']
})
export class ReserveListComponent implements OnInit {

  constructor(private reserveService: ReserveService) { }

  ngOnInit() {
    this.reserveService.get().subscribe(res => {
      console.log(res);
    });
  }

}
