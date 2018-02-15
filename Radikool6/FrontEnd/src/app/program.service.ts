import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {ProgramSearchCondition} from './program';
import {ApiResult} from "./api-result";

@Injectable()
export class ProgramService extends BaseService{

  constructor(http: HttpClient) { super(http); }

  /**
   * 番組検索
   * @param {ProgramSearchCondition} cond
   * @returns {Observable<Object>}
   */
  public search = (cond: ProgramSearchCondition) => {
    return this.http.post<ApiResult>('./api/program/', cond);
  }

}
