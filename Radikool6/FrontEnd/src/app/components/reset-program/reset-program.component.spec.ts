import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetProgramComponent } from './reset-program.component';

describe('ResetProgramComponent', () => {
  let component: ResetProgramComponent;
  let fixture: ComponentFixture<ResetProgramComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResetProgramComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetProgramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
