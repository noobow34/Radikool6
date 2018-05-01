import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ApiResult} from '../interfaces/api-result';
import {ReserveTask} from '../interfaces/reserveTask';

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

  /**
   * 停止／再開
   * @param {ReserveTask} task
   * @returns {Observable<Object>}
   */
  public stopRestart = (task: ReserveTask) => {
    return this.http.post<ApiResult>(`./api/task/${task.id}`, {});
  }
}
