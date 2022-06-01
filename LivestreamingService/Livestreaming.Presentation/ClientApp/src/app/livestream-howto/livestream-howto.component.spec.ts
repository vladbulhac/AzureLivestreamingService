import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestreamHowtoComponent } from './livestream-howto.component';

describe('LivestreamHowtoComponent', () => {
  let component: LivestreamHowtoComponent;
  let fixture: ComponentFixture<LivestreamHowtoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LivestreamHowtoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LivestreamHowtoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
