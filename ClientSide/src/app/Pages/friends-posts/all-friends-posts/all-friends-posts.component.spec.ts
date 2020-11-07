import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllFriendsPostsComponent } from './all-friends-posts.component';

describe('AllFriendsPostsComponent', () => {
  let component: AllFriendsPostsComponent;
  let fixture: ComponentFixture<AllFriendsPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllFriendsPostsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AllFriendsPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
