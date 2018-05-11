import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'time'
})
export class TimePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (!value){
      return null;
    }
    let date = moment(value);
    let hour = date.hour();
    if (hour < 5){
      hour += 24;
    }

    return `${('00' + hour).substr(-2)}:${date.format('mm')}`;
  }

}
