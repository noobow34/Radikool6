import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BaseService} from './base.service';
import {ApiResult} from '../interfaces/api-result';
import {SystemInfo} from '../interfaces/system-info';

@Injectable()
export class SystemInfoService extends BaseService {
  constructor(http: HttpClient) {
    super(http);
  }

  public get = () => {
    return this.http.get<ApiResult<SystemInfo>>('./api/system-info');
  }
}
