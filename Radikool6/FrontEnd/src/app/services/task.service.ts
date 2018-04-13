import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {Station} from "../interfaces/station";
import {ApiResult} from "../interfaces/api-result";

@Injectable()
export class TaskService extends BaseService{

  constructor(http: HttpClient) { super(http); }

  /**
   * 録音状態取得
   * @returns {Observable<Object>}
   */
  public get = () => {
    return this.http.get<ApiResult>('./api/task/');
  }
}
