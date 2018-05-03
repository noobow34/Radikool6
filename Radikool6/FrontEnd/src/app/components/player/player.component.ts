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
  public show = false;

  public position = 0;
  public volume = 0.5;
  public duration = 0;

  private audio;

  constructor(private stateService: StateService) {
  }

  ngOnInit() {
    this.subs.push(this.stateService.playLibrary.subscribe(value => {
      this.library = value;
      this.audio = new Audio(`./library/play/${this.library.id}`);
      this.show = true;

      this.audio.addEventListener('loadedmetadata', e => {
        this.duration = this.audio.duration;
        this.audio.volume = this.volume;
      });

      this.audio.addEventListener('canplay', e => {
        this.audio.play();
      });


      this.audio.addEventListener('timeupdate', e => {
        this.position = this.audio.currentTime;
      });

    }));
  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

  public play = () => {
    this.audio.play();
  }

  public pause = () => {
    this.audio.pause();
  }

  /**
   * シークバー
   * @param e
   */
  public changePosition = (e) => {
    this.audio.currentTime = e.value;
  }

  /**
   * 音量変更
   * @param e
   */
  public changeVolume = (e) => {
    this.volume = e.value;
    this.audio.volume = e.value;
  }

}
