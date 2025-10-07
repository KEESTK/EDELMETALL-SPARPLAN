import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

export type AppConfig = { apiUrl: string };

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private cfg: AppConfig | null = null;

  constructor(private http: HttpClient) {}

  async load(): Promise<void> {
    try {
      const response = await firstValueFrom(this.http.get<AppConfig>('/assets/config.json'));
      this.cfg = response;
      console.log('[ConfigService] Loaded config:', this.cfg);
    } catch (err) {
      console.error('[ConfigService] Failed to load config.json:', err);
      this.cfg = { apiUrl: 'http://localhost:5001/api' }; // fallback
    }
  }

  get(): AppConfig {
    if (!this.cfg) throw new Error('App config not loaded yet');
    return this.cfg;
  }

  get apiUrl(): string {
    return this.cfg?.apiUrl ?? 'http://localhost:5001/api';
  }
}
