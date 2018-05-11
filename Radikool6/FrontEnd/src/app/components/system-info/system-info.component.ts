import { Component, OnInit } from '@angular/core';
import {SystemInfoService} from '../../services/system-info.service';
import {SystemInfo} from '../../interfaces/system-info';

@Component({
  selector: 'app-system-info',
  templateUrl: './system-info.component.html',
  styleUrls: ['./system-info.component.scss']
})
export class SystemInfoComponent implements OnInit {

  public systemInfo: SystemInfo = {};

  constructor(private systemInfoService: SystemInfoService) {
  }

  ngOnInit() {
    this.systemInfoService.get().subscribe(res => {
      if (res.result) {
        this.systemInfo = res.data;
      }
    });
  }

}
