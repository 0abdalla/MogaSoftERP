import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralSelectorComponent } from './general-selector.component';

describe('GeneralSelectorComponent', () => {
  let component: GeneralSelectorComponent;
  let fixture: ComponentFixture<GeneralSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GeneralSelectorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GeneralSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
