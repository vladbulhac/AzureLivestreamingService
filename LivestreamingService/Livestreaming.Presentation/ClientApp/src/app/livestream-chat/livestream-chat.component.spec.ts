import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestreamChatComponent } from './livestream-chat.component';

describe('LivestreamChatComponent', () => {
  let component: LivestreamChatComponent;
  let fixture: ComponentFixture<LivestreamChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LivestreamChatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LivestreamChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
