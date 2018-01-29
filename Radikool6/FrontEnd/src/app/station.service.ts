import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ApiResult} from './api-result';

@Injectable()
export class StationService extends BaseService{

  constructor(http: HttpClient) { super(http); }

  /**
   * 放送局取得
   * @returns {Observable<Object>}
   */
  public get = () => {
    return this.http.get<ApiResult>('./api/station/');
  }

}
