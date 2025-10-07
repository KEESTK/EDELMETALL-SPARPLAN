import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '../../core/config.service';
import { Observable } from 'rxjs';
import { SparplanCreateDto, DepotDto } from '../../types/models';

@Injectable({ providedIn: 'root' })
export class DepotsService {
  private readonly apiUrl: string;

  constructor(private http: HttpClient, private config: ConfigService) {
    this.apiUrl = `${this.config.apiUrl}/Depots`;
  }

  /** Create a new depot */
  create(): Observable<DepotDto> {
    return this.http.post<DepotDto>(this.apiUrl, {});
  }

  /** Add a Sparplan to an existing Depot */
  addSparplan(depotId: string, dto: SparplanCreateDto): Observable<DepotDto> {
  return this.http.post<DepotDto>(`${this.apiUrl}/${depotId}/add-sparplan`, dto);
  }

  /** Get all depots */
  getAll(): Observable<DepotDto[]> {
    return this.http.get<DepotDto[]>(this.apiUrl);
  }

  /** Get single depot by ID */
  getById(id: string): Observable<DepotDto> {
    return this.http.get<DepotDto>(`${this.apiUrl}/${id}`);
  }
}
