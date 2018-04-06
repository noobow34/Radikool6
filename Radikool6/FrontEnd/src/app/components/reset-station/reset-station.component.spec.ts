import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetStationComponent } from './reset-station.component';

describe('ResetStationComponent', () => {
  let component: ResetStationComponent;
  let fixture: ComponentFixture<ResetStationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResetStationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetStationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
