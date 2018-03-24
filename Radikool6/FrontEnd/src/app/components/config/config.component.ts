import { Component, OnInit } from '@angular/core';
import {ConfigService} from '../../services/config.service';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.scss']
})
export class ConfigComponent implements OnInit {

  constructor(private configService: ConfigService) { }

  ngOnInit() {
    this.configService.getEncodeSettings().subscribe(res => {
      console.log(res);
    });
  }

}
