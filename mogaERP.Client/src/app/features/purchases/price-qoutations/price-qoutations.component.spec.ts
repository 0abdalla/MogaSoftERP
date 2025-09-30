import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PriceQoutationsComponent } from './price-qoutations.component';

describe('PriceQoutationsComponent', () => {
  let component: PriceQoutationsComponent;
  let fixture: ComponentFixture<PriceQoutationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PriceQoutationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PriceQoutationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
