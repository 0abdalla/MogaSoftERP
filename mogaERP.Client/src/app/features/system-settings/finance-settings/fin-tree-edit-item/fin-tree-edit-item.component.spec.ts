import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinTreeEditItemComponent } from './fin-tree-edit-item.component';

describe('FinTreeEditItemComponent', () => {
  let component: FinTreeEditItemComponent;
  let fixture: ComponentFixture<FinTreeEditItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FinTreeEditItemComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FinTreeEditItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
