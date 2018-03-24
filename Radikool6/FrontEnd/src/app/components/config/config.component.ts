import { Component, OnInit } from '@angular/core';
import {ConfigService} from '../../services/config.service';
import {Config, EncodeSetting} from '../../interfaces/config';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.scss']
})
export class ConfigComponent implements OnInit {

  public config: Config = {};
  public encodeSettings: EncodeSetting[] = [];

  constructor(private configService: ConfigService) { }

  ngOnInit() {
    this.configService.getEncodeSettings().subscribe(res => {
      if (res.result){
        this.encodeSettings = res.data;
      }
    });

    this.configService.get().subscribe(res => {
      if (res.result){
        this.config = res.data;
      }
    });

  }

  public save = () => {
    this.configService.update(this.config).subscribe(res => {
      console.log(res);
    });
  }

}
