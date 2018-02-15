import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import {HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule, MatDialogModule, MatExpansionModule, MatToolbarModule} from '@angular/material';
import { ContentComponent } from './content/content.component';
import {StateService} from './state.service';
import { TimetableComponent } from './timetable/timetable.component';
import { LibraryComponent } from './library/library.component';
import { SettingComponent } from './setting/setting.component';
import { RadioPlayerComponent } from './radio-player/radio-player.component';
import {StationService} from './station.service';
import {ProgramService} from './program.service';
import { ReserveEditComponent } from './reserve-edit/reserve-edit.component';
import { ReserveListComponent } from './reserve-list/reserve-list.component';
import {ReserveService} from './reserve.service';


@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    ContentComponent,
    TimetableComponent,
    LibraryComponent,
    SettingComponent,
    RadioPlayerComponent,
    ReserveEditComponent,
    ReserveListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatExpansionModule,
    MatDialogModule

  ],
  providers: [
    StateService,
    StationService,
    ProgramService,
    ReserveService
    ],
  entryComponents:[
    ReserveEditComponent
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
