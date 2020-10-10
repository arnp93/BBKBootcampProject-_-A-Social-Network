import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WhosFollowingComponent } from './whos-following.component';

describe('WhosFollowingComponent', () => {
  let component: WhosFollowingComponent;
  let fixture: ComponentFixture<WhosFollowingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WhosFollowingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WhosFollowingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
