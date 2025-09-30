import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangePermissionsComponent } from './exchange-permissions.component';

describe('ExchangePermissionsComponent', () => {
  let component: ExchangePermissionsComponent;
  let fixture: ComponentFixture<ExchangePermissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ExchangePermissionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ExchangePermissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
