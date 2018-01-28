import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class ReserveService extends BaseService{

  constructor(http: HttpClient) { super(http); }

}
