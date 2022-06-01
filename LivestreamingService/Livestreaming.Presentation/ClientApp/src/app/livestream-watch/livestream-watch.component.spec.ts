import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestreamWatchComponent } from './livestream-watch.component';

describe('LivestreamWatchComponent', () => {
  let component: LivestreamWatchComponent;
  let fixture: ComponentFixture<LivestreamWatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LivestreamWatchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LivestreamWatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
