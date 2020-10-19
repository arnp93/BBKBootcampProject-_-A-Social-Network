import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllUsersPostsComponent } from './all-users-posts.component';

describe('AllUsersPostsComponent', () => {
  let component: AllUsersPostsComponent;
  let fixture: ComponentFixture<AllUsersPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllUsersPostsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AllUsersPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
