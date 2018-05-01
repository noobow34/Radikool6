import {Component, OnDestroy, OnInit, ViewChild, ViewChildren} from '@angular/core';
import {ReserveService} from '../../services/reserve.service';
import {Reserve} from '../../interfaces/reserve';
import {StateService} from '../../services/state.service';
import {MatSort, MatTable, MatTableDataSource} from '@angular/material';
import {TaskService} from '../../services/task.service';
import {ReserveTask} from '../../interfaces/reserveTask';
import {Observable} from 'rxjs/Rx';

@Component({
  selector: 'app-reserve-list',
  templateUrl: './reserve-list.component.html',
  styleUrls: ['./reserve-list.component.scss']
})
export class ReserveListComponent implements OnInit, OnDestroy {

  public reserves: Reserve[] = [];
  public tasks: ReserveTask[] = [];

  private timer;
  private subs = [];

  // mat-table
  public reserveDataSource = new MatTableDataSource();
  public reserveDisplayedColumns = ['name', 'stationName', 'start', 'end'];
  @ViewChild('reserveSort') reserveSort: MatSort;

  // mat-table
  public taskDataSource = new MatTableDataSource();
  public taskDisplayedColumns = ['start', 'end', 'status'];
  @ViewChild('taskSort') taskSort: MatSort;

  constructor(private reserveService: ReserveService,
              private taskService: TaskService,
              public stateService: StateService) {
  }

  ngOnInit() {
    this.init();

  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

  private init = () => {
    this.reserveService.get().subscribe(res => {
      if (res.result) {
        this.reserves = res.data;
        this.reserveDataSource = new MatTableDataSource(this.reserves);
        this.reserveDataSource.sort = this.reserveSort;

        console.log(this.reserveSort);
      }
    });

    this.timer = Observable.timer(0, 1000);
    this.subs.push(this.timer.subscribe(x => {
      this.taskService.get().subscribe(res => {
        if (res.result) {
          this.tasks = res.data;

          this.taskDataSource = new MatTableDataSource(this.tasks);
          this.taskDataSource.sort = this.taskSort;
        }
      });
    }));
    /*this.tasks = [
      {start: new Date('2018-04-30 00:00:00'), end: new Date('2018-04-30 01:00:00'), status: 'status'},
      {start: new Date('2018-04-30 01:00:00'), end: new Date('2018-04-30 02:00:00'), status: 'status2'}];
    this.taskDataSource = new MatTableDataSource(this.tasks);
    this.taskDataSource.sort = this.taskSort;
    console.log(this.taskDataSource);*/
  }

  /**
   * 予約編集
   * @param {Reserve} reserve
   */
  public editReserve = (reserve: Reserve) => {
    this.stateService.editReserve({reserve: reserve}, (res) => {
      if (res) {
        this.init();
      }
    });
  }

  public stopRestartReserveTask = (task: ReserveTask) => {
    this.taskService.stopRestart(task).subscribe(res => {
console.log(res);
    });
  }

}
