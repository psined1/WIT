import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductInquiryComponent } from './product-inquiry.component';

describe('ProductInquiryComponent', () => {
  let component: ProductInquiryComponent;
  let fixture: ComponentFixture<ProductInquiryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductInquiryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductInquiryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
