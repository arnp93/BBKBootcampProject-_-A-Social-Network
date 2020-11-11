import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HashtagPostsMainPageComponent } from './hashtag-posts-main-page.component';

describe('HashtagPostsMainPageComponent', () => {
  let component: HashtagPostsMainPageComponent;
  let fixture: ComponentFixture<HashtagPostsMainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HashtagPostsMainPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HashtagPostsMainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
