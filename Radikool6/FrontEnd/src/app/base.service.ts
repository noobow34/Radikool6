import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class BaseService {

  constructor(protected http: HttpClient) { }

}
