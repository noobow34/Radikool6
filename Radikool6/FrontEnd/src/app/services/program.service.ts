import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {Program, ProgramSearchCondition} from '../interfaces/program';
import {ApiResult} from '../interfaces/api-result';

@Injectable()
export class ProgramService extends BaseService{

  constructor(http: HttpClient) { super(http); }

  /**
   * 番組検索
   * @param {ProgramSearchCondition} cond
   * @returns {Observable<Object>}
   */
  public search = (cond: ProgramSearchCondition) => {
    return this.http.post<ApiResult<{ programs: Program[], range: Date[]}>>('./api/program/', cond);
  }

}
