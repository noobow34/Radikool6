import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ApiResult} from './api-result';

@Injectable()
export class StationService extends BaseService{

  constructor(http: HttpClient) { super(http); }


  /**
   * 放送局再取得
   * @param {string} type
   * @returns {Observable<Object>}
   */
  public refresh = (type: string) => {
    return this.http.post<ApiResult>(`./api/station/${type}`, {});
  }

  /**
   * 放送局取得
   * @returns {Observable<Object>}
   */
  public get = () => {
    return this.http.get<ApiResult>('./api/station/');
  }

}
