import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HashtagPostsComponent } from './hashtag-posts.component';

describe('HashtagPostsComponent', () => {
  let component: HashtagPostsComponent;
  let fixture: ComponentFixture<HashtagPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HashtagPostsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HashtagPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
