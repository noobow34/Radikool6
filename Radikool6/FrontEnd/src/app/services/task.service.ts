import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class TaskService extends BaseService{

  constructor(http: HttpClient) { super(http); }

}
