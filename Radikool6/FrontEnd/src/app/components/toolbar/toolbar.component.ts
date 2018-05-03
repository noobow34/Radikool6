import {Component, OnDestroy, OnInit} from '@angular/core';
import {StateService} from '../../services/state.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit, OnDestroy {

  public selectedContent;
  private subs = [];

  constructor(private stateService: StateService) {
  }

  ngOnInit() {
    this.subs.push(this.stateService.selectedContent.subscribe(value => {
      this.selectedContent = value;
    }));
  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

  public setContent = (content: string) => {
    this.stateService.selectedContent.next(content);
  }

}
