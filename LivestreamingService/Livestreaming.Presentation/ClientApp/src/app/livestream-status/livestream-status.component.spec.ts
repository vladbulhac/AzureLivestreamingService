import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestreamStatusComponent } from './livestream-status.component';

describe('LivestreamStatusComponent', () => {
  let component: LivestreamStatusComponent;
  let fixture: ComponentFixture<LivestreamStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LivestreamStatusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LivestreamStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
