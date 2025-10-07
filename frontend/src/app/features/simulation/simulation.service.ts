import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService } from '../../core/config.service';
import { SimulationResult } from '../../types/models';
import { MetalType } from '../../types/enums';

@Injectable({ providedIn: 'root' })
export class SimulationService {
  private readonly apiUrl: string;

  constructor(private http: HttpClient, private config: ConfigService) {
    this.apiUrl = `${this.config.apiUrl}/Simulation`;
  }

  simulate(
    metal: MetalType,
    monthlyRate: number,
    from: string,
    to: string
  ): Observable<SimulationResult[]> {
    const params = { metal, monthlyRate, from, to };
    return this.http.get<SimulationResult[]>(this.apiUrl, { params });
  }
}
