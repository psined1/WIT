import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationSheetComponent } from './observation-sheet.component';

describe('ObservationSheetComponent', () => {
  let component: ObservationSheetComponent;
  let fixture: ComponentFixture<ObservationSheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationSheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
