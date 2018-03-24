import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import {HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {
  MatButtonModule, MatDialogModule, MatExpansionModule, MatListModule, MatProgressSpinnerModule, MatSelectModule,
  MatToolbarModule
} from '@angular/material';
import { ContentComponent } from './components/content/content.component';
import {StateService} from './services/state.service';
import { TimetableComponent } from './components/timetable/timetable.component';
import { LibraryComponent } from './components/library/library.component';
import { RadioPlayerComponent } from './components/radio-player/radio-player.component';
import {StationService} from './services/station.service';
import {ProgramService} from './services/program.service';
import { ReserveEditComponent } from './components/reserve-edit/reserve-edit.component';
import { ReserveListComponent } from './components/reserve-list/reserve-list.component';
import {ReserveService} from './services/reserve.service';
import { TimePipe } from './pipes/time.pipe';
import {FormsModule} from '@angular/forms';
import {ConfigService} from './services/config.service';
import { ConfigComponent } from './components/config/config.component';


@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    ContentComponent,
    TimetableComponent,
    LibraryComponent,
    RadioPlayerComponent,
    ReserveEditComponent,
    ReserveListComponent,
    TimePipe,
    ConfigComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatExpansionModule,
    MatDialogModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatSelectModule
  ],
  providers: [
    StateService,
    StationService,
    ProgramService,
    ReserveService,
    ConfigService
    ],
  entryComponents:[
    ReserveEditComponent
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
