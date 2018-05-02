import { Component, OnInit } from '@angular/core';
import {ConfigService} from '../../services/config.service';
import {Config } from '../../interfaces/config';
import {StateService} from '../../services/state.service';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.scss']
})
export class ConfigComponent implements OnInit {

  public config: Config = {};

  constructor(private stateService: StateService,
              private configService: ConfigService) {
  }

  ngOnInit() {
    this.configService.get().subscribe(res => {
      if (res.result) {
        this.config = res.data;
      }
    });

  }

  /**
   * 置換
   * @param {string} property
   */
  public macro = (property: string) => {
    this.stateService.macro(this.config[property], res => {
      if (res) {
        this.config[property] = res;
      }
    });
  }

  public save = () => {
    this.configService.update(this.config).subscribe(res => {
      console.log(res);
    });
  }

}
