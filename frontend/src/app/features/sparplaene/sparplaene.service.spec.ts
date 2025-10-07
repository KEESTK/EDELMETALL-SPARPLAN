import { TestBed } from '@angular/core/testing';

import { SparplaeneService } from './sparplaene.service';

describe('SparplaeneService', () => {
  let service: SparplaeneService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SparplaeneService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
