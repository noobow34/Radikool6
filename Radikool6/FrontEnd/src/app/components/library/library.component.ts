import {Component, OnInit, ViewChild} from '@angular/core';
import {LibraryService} from '../../services/library.service';
import {Library} from '../../interfaces/library';
import {MatSort, MatTableDataSource} from '@angular/material';
import {StateService} from '../../services/state.service';

@Component({
  selector: 'app-library',
  templateUrl: './library.component.html',
  styleUrls: ['./library.component.scss']
})
export class LibraryComponent implements OnInit {

  public libraries: Library[] = [];


  // mat-table
  public dataSource = new MatTableDataSource();
  public displayedColumns = ['start', 'stationName', 'title', 'size', 'created'];
  @ViewChild(MatSort) sort: MatSort;

  constructor(private stateServie: StateService,
              private libraryService: LibraryService) {
  }

  ngOnInit() {
    this.init();
  }

  /**
   * 初期化
   */
  private init = () => {
    this.libraryService.get().subscribe(res => {
      if (res.result) {
        this.libraries = res.data;

        const data = this.libraries.map(l => {
          return {
            start: l.program.start,
            stationName: l.program.station.name,
            title: l.program.title,
            size: l.size,
            created: l.created,
            orgData: l
          };
        });

        this.dataSource = new MatTableDataSource(data);
        this.dataSource.sort = this.sort;
      }
    });
  }

  /**
   * 詳細表示
   * @param {Library} library
   */
  public detail = (library: Library) => {
    this.stateServie.showLibraryDetail(library, (res) => {
      if (res) {
        this.init();
      }
    });
    // this.stateServie.playLibrary.next(library);
    //window.open(`./records/${library.path}`);
  }
}
