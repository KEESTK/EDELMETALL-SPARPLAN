import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SparplanDto, SparplanCreateDto } from '../../types/models';
import { ConfigService } from '../../core/config.service';


@Injectable({ providedIn: 'root' })
export class SparplaeneService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient, private config: ConfigService) {
    this.baseUrl = `${this.config.apiUrl}/Sparplaene`;
  }

  /** 🔹 Create a new Sparplan (requires valid DepotId) */
  create(dto: SparplanCreateDto): Observable<SparplanDto> {
    return this.http.post<SparplanDto>(this.baseUrl, dto);
  }


  /** 🔹 Get all Sparpläne */
  getAll(): Observable<SparplanDto[]> {
    return this.http.get<SparplanDto[]>(this.baseUrl);
  }

  /** 🔹 Get a specific Sparplan by ID */
  getById(id: string): Observable<SparplanDto> {
    return this.http.get<SparplanDto>(`${this.baseUrl}/${id}`);
  }
}
