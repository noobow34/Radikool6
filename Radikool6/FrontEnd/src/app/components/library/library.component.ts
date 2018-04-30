import {Component, OnInit, ViewChild} from '@angular/core';
import {LibraryService} from '../../services/library.service';
import {Library} from '../../interfaces/library';
import {MatSort, MatTableDataSource} from '@angular/material';

@Component({
  selector: 'app-library',
  templateUrl: './library.component.html',
  styleUrls: ['./library.component.scss']
})
export class LibraryComponent implements OnInit {

  public libraries: Library[] = [];


  // mat-table
  public dataSource = new MatTableDataSource();
  public displayedColumns = ['fileName'];
  @ViewChild(MatSort) sort: MatSort;

  constructor(private libraryService: LibraryService) {
  }

  ngOnInit() {
    this.libraryService.get().subscribe(res => {
      if (res.result) {
        this.libraries = res.data;
        this.dataSource = new MatTableDataSource(this.libraries);
        this.dataSource.sort = this.sort;
      }
    });
  }

  public play = (library: Library) => {
    window.open(`./library/play/${library.id}`);
  }

}
