import {Component, OnDestroy, OnInit, ViewChild, ViewChildren} from '@angular/core';
import {ReserveService} from '../../services/reserve.service';
import {Reserve} from '../../interfaces/reserve';
import {StateService} from '../../services/state.service';
import {
  DateAdapter,  MatSort,  MatTableDataSource,
  NativeDateAdapter
} from '@angular/material';
import {TaskService} from '../../services/task.service';
import {ReserveTask} from '../../interfaces/reserveTask';
import {Observable} from 'rxjs/Rx';

@Component({
  selector: 'app-reserve-list',
  templateUrl: './reserve-list.component.html',
  styleUrls: ['./reserve-list.component.scss']})
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
              dateAdapter: DateAdapter<NativeDateAdapter>,
              public stateService: StateService) {
    dateAdapter.setLocale('ja');
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

    if (!this.timer) {
      this.timer = Observable.timer(0, 10 * 1000);
      this.subs.push(this.timer.subscribe(x => {
        this.taskService.get().subscribe(res => {
          if (res.result) {
            this.tasks = res.data;

            this.taskDataSource = new MatTableDataSource(this.tasks);
            this.taskDataSource.sort = this.taskSort;
          }
        });
      }));
    }
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
    });
  }

}
