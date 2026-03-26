import { TestBed } from '@angular/core/testing';

import { MsalConfig } from './msal.config';

describe('MsalConfig', () => {
  let service: MsalConfig;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsalConfig);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
