import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import {HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {
  MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule, MatDialogModule, MatExpansionModule,
  MatInputModule, MatListModule, MatNativeDateModule,
  MatProgressSpinnerModule,
  MatSelectModule, MatSortModule, MatTableModule,
  MatToolbarModule
} from '@angular/material';
import { ContentComponent } from './components/content/content.component';
import {StateService} from './services/state.service';
import { TimetableComponent } from './components/timetable/timetable.component';
import { LibraryComponent } from './components/library/library.component';
import {StationService} from './services/station.service';
import {ProgramService} from './services/program.service';
import { ReserveEditComponent } from './components/reserve-edit/reserve-edit.component';
import { ReserveListComponent } from './components/reserve-list/reserve-list.component';
import {ReserveService} from './services/reserve.service';
import { TimePipe } from './pipes/time.pipe';
import {FormsModule} from '@angular/forms';
import {ConfigService} from './services/config.service';
import { ConfigComponent } from './components/config/config.component';
import { ManageComponent } from './components/manage/manage.component';
import { ResetProgramComponent } from './components/reset-program/reset-program.component';
import { ResetStationComponent } from './components/reset-station/reset-station.component';
import {TaskService} from './services/task.service';
import {LibraryService} from './services/library.service';
import { PlayerComponent } from './components/player/player.component';
import { MacroComponent } from './components/macro/macro.component';


@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    ContentComponent,
    TimetableComponent,
    LibraryComponent,
    ReserveEditComponent,
    ReserveListComponent,
    TimePipe,
    ConfigComponent,
    ManageComponent,
    ResetProgramComponent,
    ResetStationComponent,
    PlayerComponent,
    MacroComponent
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
    MatSelectModule,
    MatInputModule,
    MatCardModule,
    MatCheckboxModule,
    MatTableModule,
    MatSortModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [
    StateService,
    StationService,
    ProgramService,
    ReserveService,
    ConfigService,
    TaskService,
    LibraryService
  ],
  entryComponents: [
    ReserveEditComponent,
    MacroComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
