import {Component, OnDestroy, OnInit, ValueProvider} from '@angular/core';
import {StateService} from '../state.service';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss']
})
export class ContentComponent implements OnInit, OnDestroy {

  public selectedContent = '';

  private subs = [];

  constructor(private stateService: StateService) { }

  ngOnInit() {
    this.subs.push(this.stateService.selectedContent.subscribe(value => this.selectedContent = value));
  }

  ngOnDestroy(){
    this.subs.forEach(s => s.unsubscribe());
  }

}
