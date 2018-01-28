import { Component, OnInit } from '@angular/core';
import {StateService} from "../state.service";

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {

  constructor(private stateService: StateService) { }

  ngOnInit() {
  }

  public setContent = (content:string) => {
    this.stateService.selectedContent.next(content);
  }

}
