import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ApiResult} from '../interfaces/api-result';
import {Station} from '../interfaces/station';

@Injectable()
export class StationService extends BaseService{

  constructor(http: HttpClient) { super(http); }


  /**
   * 放送局再取得
   * @param {string} type
   * @returns {Observable<Object>}
   */
  public refresh = (type: string) => {
    return this.http.post<ApiResult<Station[]>>(`./api/station/${type}`, {});
  }

  /**
   * 放送局取得
   * @returns {Observable<Object>}
   */
  public get = (types: string[]) => {
    return this.http.post<ApiResult<{[key: string]: Station[]}>>(`./api/station/`, { types: types });
  }

}
