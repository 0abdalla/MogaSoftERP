import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReciptPermissionsComponent } from './recipt-permissions.component';

describe('ReciptPermissionsComponent', () => {
  let component: ReciptPermissionsComponent;
  let fixture: ComponentFixture<ReciptPermissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReciptPermissionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReciptPermissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
