import { Component, OnInit } from '@angular/core';
import {StationService} from '../../services/station.service';
import {Station} from '../../interfaces/station';

@Component({
  selector: 'app-reset-station',
  templateUrl: './reset-station.component.html',
  styleUrls: ['./reset-station.component.scss']
})
export class ResetStationComponent implements OnInit {
  public loading = false;
  public stations: Station[] = [];

  constructor(private stationService: StationService) { }

  ngOnInit() {
    this.stationService.get('radiko').subscribe(res => {
      if (res.result){
        this.stations = res.data;
      }
    });
  }

  public reset = (type: string) => {
    this.stationService.refresh(type).subscribe(res => {
      if (res.result) {
        this.stations = res.data;
      }
    });
  }

}
