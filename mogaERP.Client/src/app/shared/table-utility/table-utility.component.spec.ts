import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableUtilityComponent } from './table-utility.component';

describe('TableUtilityComponent', () => {
  let component: TableUtilityComponent;
  let fixture: ComponentFixture<TableUtilityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TableUtilityComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TableUtilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
