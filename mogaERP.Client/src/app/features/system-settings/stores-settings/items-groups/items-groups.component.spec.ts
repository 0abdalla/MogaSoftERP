import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemsGroupsComponent } from './items-groups.component';

describe('ItemsGroupsComponent', () => {
  let component: ItemsGroupsComponent;
  let fixture: ComponentFixture<ItemsGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemsGroupsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ItemsGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
