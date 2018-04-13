import {Component, OnInit, ViewChild} from '@angular/core';
import {ReserveService} from '../../services/reserve.service';
import {Reserve} from '../../interfaces/reserve';
import {StateService} from '../../services/state.service';
import {MatSort, MatTableDataSource} from '@angular/material';
import {TaskService} from '../../services/task.service';
import {ReserveTask} from '../../interfaces/reserveTask';

@Component({
  selector: 'app-reserve-list',
  templateUrl: './reserve-list.component.html',
  styleUrls: ['./reserve-list.component.scss']
})
export class ReserveListComponent implements OnInit {

  public reserves: Reserve[] = [];
  public tasks:ReserveTask[] = [];

  // mat-table
  public dataSource = new MatTableDataSource();
  public displayedColumns = ['name', 'stationName', 'start', 'end'];
  @ViewChild(MatSort) sort: MatSort;


  constructor(
    private reserveService: ReserveService,
    private taskService: TaskService,
    public stateService: StateService) { }

  ngOnInit() {
    this.init();

  }


  private init = () => {
    this.reserveService.get().subscribe(res => {
      if (res.result){
        this.reserves = res.data;
        this.dataSource = new MatTableDataSource(this.reserves);
        this.dataSource.sort = this.sort;
      }
    });
console.log(this.taskService.get());

    setInterval(() => {
      this.taskService.get().subscribe(res => {
        if(res.result){
          this.tasks = res.data;
        }
      });

    }, 1000);
  }

  /**
   * 予約編集
   * @param {Reserve} reserve
   */
  public editReserve = (reserve: Reserve) => {
    this.stateService.editReserve({ reserve: reserve}, (res) => {
      if (res){
        this.init();
      }
    });
  }

}
