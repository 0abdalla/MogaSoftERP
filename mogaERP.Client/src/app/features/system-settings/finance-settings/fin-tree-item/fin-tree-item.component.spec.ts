import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinTreeItemComponent } from './fin-tree-item.component';

describe('FinTreeItemComponent', () => {
  let component: FinTreeItemComponent;
  let fixture: ComponentFixture<FinTreeItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FinTreeItemComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FinTreeItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
