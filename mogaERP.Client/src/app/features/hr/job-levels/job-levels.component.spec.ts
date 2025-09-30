import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobLevelsComponent } from './job-levels.component';

describe('JobLevelsComponent', () => {
  let component: JobLevelsComponent;
  let fixture: ComponentFixture<JobLevelsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [JobLevelsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(JobLevelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
