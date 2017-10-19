import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmYesNoComponent } from './confirm-yes-no.component';

describe('ConfirmYesNoComponent', () => {
  let component: ConfirmYesNoComponent;
  let fixture: ComponentFixture<ConfirmYesNoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmYesNoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmYesNoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
