import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import {HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule, MatExpansionModule, MatToolbarModule} from '@angular/material';
import { ContentComponent } from './content/content.component';
import {StateService} from './state.service';
import { TimetableComponent } from './timetable/timetable.component';
import { LibraryComponent } from './library/library.component';
import { SettingComponent } from './setting/setting.component';
import { ReserveComponent } from './reserve/reserve.component';
import { RadioPlayerComponent } from './radio-player/radio-player.component';
import {StationService} from './station.service';


@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    ContentComponent,
    TimetableComponent,
    LibraryComponent,
    SettingComponent,
    ReserveComponent,
    RadioPlayerComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatExpansionModule,

  ],
  providers: [
    StateService,
    StationService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
