import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable , map} from 'rxjs';
import {
  TransactionDepositDto,
  TransactionFeeDto,
  TransactionCloseRequestDto,
  TransactionCloseConfirmDto,
  TransactionDto
} from '../../types/models';
import { ConfigService } from '../../core/config.service';

@Injectable({ providedIn: 'root' })
export class TransactionsService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient, private config: ConfigService) {
    this.baseUrl = `${this.config.apiUrl}/transactions`;
  }

  /** 📥 Einzahlen auf Sparplan */
  deposit(dto: TransactionDepositDto): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/deposit`, dto);
  }

  /** 💸 Quartalsgebühr abbuchen */
  fee(dto: TransactionFeeDto): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/fee`, dto);
  }

  /** 🧾 Schließung anfordern (Token erhalten) */
  requestClose(dto: TransactionCloseRequestDto): Observable<{
    sessionToken: string;
    metal: string;
    bars: number;
    estimatedPayout: number;
    message: string;
  }> {
    return this.http
      .post<any>(`${this.baseUrl}/close/request`, dto)
      .pipe(
        map((res) => ({
          sessionToken: res.sessionToken ?? res.SessionToken,
          metal: res.metal ?? res.Metal,
          bars: res.bars ?? res.Bars,
          estimatedPayout: res.estimatedPayout ?? res.EstimatedPayout,
          message: res.message ?? res.Message,
        }))
      );
  }


  /** ✅ Schließung bestätigen */
  confirmClose(dto: TransactionCloseConfirmDto): Observable<{
    message: string;
    paidOut: number;
    bankAccount: string;
  }> {
    return this.http.post<{
      message: string;
      paidOut: number;
      bankAccount: string;
    }>(`${this.baseUrl}/close/confirm`, dto);
  }

  /** 🔍 Alle Transaktionen eines Sparplans abrufen */
  getTransactions(sparplanId: string): Observable<TransactionDto[]> {
    return this.http.get<TransactionDto[]>(`${this.baseUrl}/${sparplanId}`);
  }
}
