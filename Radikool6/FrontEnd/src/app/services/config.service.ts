import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ApiResult} from '../interfaces/api-result';
import {Config, EncodeSetting} from '../interfaces/config';

@Injectable()
export class ConfigService extends BaseService {

  constructor(http:HttpClient) { super(http); }


  public get = () => {
    return this.http.get<ApiResult<Config>>('./api/config/');
  }

  public update = (config: Config) => {
    return this.http.post<ApiResult>('./api/config', config);
  }

  public getEncodeSettings = () => {
    return this.http.get<ApiResult<EncodeSetting[]>>('./api/encode_settings/');
  }

}
