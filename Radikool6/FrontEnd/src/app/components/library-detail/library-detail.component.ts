import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Library} from '../../interfaces/library';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {LibraryService} from '../../services/library.service';

@Component({
  selector: 'app-library-detail',
  templateUrl: './library-detail.component.html',
  styleUrls: ['./library-detail.component.scss']
})
export class LibraryDetailComponent implements OnInit {

  public description;

  constructor(public dialogRef: MatDialogRef<LibraryDetailComponent>,
              @Inject(MAT_DIALOG_DATA) public data: Library,
              private sanitizer: DomSanitizer,
              private libraryService: LibraryService) {
  }

  ngOnInit() {
    this.description = this.sanitizer.bypassSecurityTrustHtml(this.data.program.description);
  }

  /**
   * 再生
   */
  public play = () => {
    window.open(`./records/${this.data.path}`);
  }

  /**
   * ダウンロード
   */
  public download = () => {
    location.href = `./library/download/${this.data.id}`;
  }

  /**
   * 削除
   */
  public delete = () => {
    if (confirm('削除しますか？')) {
      this.libraryService.delete(this.data.id).subscribe(res => {
        this.dialogRef.close(true);
      });
    }
  }

}
