import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionNotificationsComponent } from './addition-notifications.component';

describe('AdditionNotificationsComponent', () => {
  let component: AdditionNotificationsComponent;
  let fixture: ComponentFixture<AdditionNotificationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdditionNotificationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdditionNotificationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
