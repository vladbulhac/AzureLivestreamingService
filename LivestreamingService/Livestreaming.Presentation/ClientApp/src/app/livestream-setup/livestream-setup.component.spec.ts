import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestreamSetupComponent } from './livestream-setup.component';

describe('LivestreamSetupComponent', () => {
  let component: LivestreamSetupComponent;
  let fixture: ComponentFixture<LivestreamSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LivestreamSetupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LivestreamSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
