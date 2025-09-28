import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinTreeComponent } from './fin-tree.component';

describe('FinTreeComponent', () => {
  let component: FinTreeComponent;
  let fixture: ComponentFixture<FinTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FinTreeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FinTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
