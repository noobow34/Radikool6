import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ApiResult} from '../interfaces/api-result';

@Injectable()
export class ConfigService extends BaseService {

  constructor(http:HttpClient) { super(http); }


  public getEncodeSettings = () => {
    return this.http.get<ApiResult>('./api/encode_settings/');
  }

}
