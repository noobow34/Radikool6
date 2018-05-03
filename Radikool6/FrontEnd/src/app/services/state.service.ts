import { Injectable } from '@angular/core';
import {BaseService} from './base.service';
import {HttpClient} from '@angular/common/http';
import {Subject} from 'rxjs/Subject';
import {MatDialog} from '@angular/material';
import {ReserveEditComponent} from '../components/reserve-edit/reserve-edit.component';
import {Library} from '../interfaces/library';
import {MacroComponent} from "../components/macro/macro.component";
import {ProgressComponent} from "../components/progress/progress.component";

@Injectable()
export class StateService extends BaseService {

  public selectedContent: Subject<string> = new Subject<string>();
  public playLibrary: Subject<Library> = new Subject<Library>();

  constructor(http: HttpClient,
              public dialog: MatDialog) {
    super(http);
  }

  /**
   * 予約編集
   * @param data
   * @param callback
   */
  public editReserve = (data, callback) => {
    const dialogRef = this.dialog.open(ReserveEditComponent, {
      //width: '250px',
      disableClose: true,
      data: data
    });

    dialogRef.afterClosed().subscribe(res => {
      callback(res);
    });
  }

  /**
   * 置換
   * @param data
   * @param callback
   */
  public macro = (data, callback) => {
    const dialogRef = this.dialog.open(MacroComponent, {
      //width: '250px',
      disableClose: true,
      data: data
    });

    dialogRef.afterClosed().subscribe(res => {
      callback(res);
    });
  }

  /**
   * タイムフリー進捗表示
   * @param callback
   */
  public showTimefreeProgress = (callback) => {
    const dialogRef = this.dialog.open(ProgressComponent, {
      //width: '250px',
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe(res => {
      callback(res);
    });
  }

}
