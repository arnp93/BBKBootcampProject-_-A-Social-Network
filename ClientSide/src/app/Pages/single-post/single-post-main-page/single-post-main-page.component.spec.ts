import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SinglePostMainPageComponent } from './single-post-main-page.component';

describe('SinglePostMainPageComponent', () => {
  let component: SinglePostMainPageComponent;
  let fixture: ComponentFixture<SinglePostMainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SinglePostMainPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SinglePostMainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
