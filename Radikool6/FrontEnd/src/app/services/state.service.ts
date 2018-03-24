import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {Subject} from 'rxjs/Subject';

@Injectable()
export class StateService extends BaseService{

  public selectedContent: Subject<string> = new Subject<string>();

  constructor(http: HttpClient) {
    super(http);
  }

}
