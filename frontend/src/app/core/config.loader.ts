import { HttpClient } from '@angular/common/http';
import { ConfigService, AppConfig } from './config.service';

export function loadAppConfig(http: HttpClient, cfg: ConfigService) {
  return async () => {
    const json = await http.get<AppConfig>('/assets/config.json').toPromise();
    cfg.set(json!);
  };
}
