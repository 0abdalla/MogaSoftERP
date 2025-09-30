import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountingGuideComponent } from './accounting-guide.component';

describe('AccountingGuideComponent', () => {
  let component: AccountingGuideComponent;
  let fixture: ComponentFixture<AccountingGuideComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AccountingGuideComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AccountingGuideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
