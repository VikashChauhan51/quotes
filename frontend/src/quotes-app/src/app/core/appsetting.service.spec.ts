import { TestBed } from '@angular/core/testing';

import { AppsettingService } from './appsetting.service';

describe('AppsettingService', () => {
  let service: AppsettingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppsettingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
