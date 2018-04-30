import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Library} from '../../interfaces/library';
import {StateService} from '../../services/state.service';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit, OnDestroy {

  public library: Library;
  private subs = [];

  @ViewChild('audio')
  private audioElement;

  private audio;

  constructor(private stateService: StateService) {
  }

  ngOnInit() {

    this.audio = this.audioElement.nativeElement;

    this.subs.push(this.stateService.playLibrary.subscribe(value => {
      this.library = value;
      console.log(this.library);
      console.log(this.audio);

      this.audio.src = `./library/play/${this.library.id}`;
   //   this.audio.play();
    }));
  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

}
