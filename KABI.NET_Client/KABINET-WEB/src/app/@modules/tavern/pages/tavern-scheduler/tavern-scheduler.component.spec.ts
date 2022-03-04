import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TavernSchedulerComponent } from './tavern-scheduler.component';

describe('TavernSchedulerComponent', () => {
  let component: TavernSchedulerComponent;
  let fixture: ComponentFixture<TavernSchedulerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TavernSchedulerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TavernSchedulerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
