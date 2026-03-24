import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinMeeting } from './join-meeting';

describe('JoinMeeting', () => {
  let component: JoinMeeting;
  let fixture: ComponentFixture<JoinMeeting>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JoinMeeting]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JoinMeeting);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
