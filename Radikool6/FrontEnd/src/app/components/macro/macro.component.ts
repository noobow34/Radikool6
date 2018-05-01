import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';


@Component({
  selector: 'app-macro',
  templateUrl: './macro.component.html',
  styleUrls: ['./macro.component.scss']
})
export class MacroComponent implements OnInit {
  public text = '';
private position = 0;
private length = 0;

  constructor(public dialogRef: MatDialogRef<MacroComponent>,
              @Inject(MAT_DIALOG_DATA) private data: string) {
    this.text = data;
  }

  ngOnInit() {
  }

  public onBlur = (e) => {
    this.position = e.srcElement.selectionStart;
    this.length = e.srcElement.selectionEnd - this.position;
  }


  public add = (tag: string) => {
    this.text = this.text.substr(0, this.position) + tag + this.text.substr(this.position + this.length);
    console.log(this.text);

  }



  public update = () => {
    this.dialogRef.close(this.text);
  }
}
