import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DebitNotificationsComponent } from './debit-notifications.component';

describe('DebitNotificationsComponent', () => {
  let component: DebitNotificationsComponent;
  let fixture: ComponentFixture<DebitNotificationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DebitNotificationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DebitNotificationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
