import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestreamHistoryComponent } from './livestream-history.component';

describe('LivestreamHistoryComponent', () => {
  let component: LivestreamHistoryComponent;
  let fixture: ComponentFixture<LivestreamHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LivestreamHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LivestreamHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
