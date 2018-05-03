import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ProgramService} from '../../services/program.service';
import {Observable} from 'rxjs/Rx';

@Component({
  selector: 'app-progress',
  templateUrl: './progress.component.html',
  styleUrls: ['./progress.component.scss']
})
export class ProgressComponent implements OnInit, OnDestroy {

  public progress = 0;

  private timer;
  private sub;

  constructor(public dialogRef: MatDialogRef<ProgressComponent>,
              private programService: ProgramService) {
  }

  ngOnInit() {
    this.timer = Observable.timer(0, 1000);
    this.sub = this.timer.subscribe(x => {
      this.programService.getTimeFreeProgress().subscribe(res => {
        if (res.result) {
          this.progress = res.data;
          if (res.data < 0) {
            this.sub.unsubscribe();
            this.dialogRef.close();
          }
        }
      });
    });

  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
