import { Component } from '@angular/core';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TransactionsService } from './transactions.service';
import {
  TransactionDto,
  TransactionDepositDto,
  TransactionFeeDto,
  TransactionCloseRequestDto,
  TransactionCloseConfirmDto,
} from '../../types/models';

@Component({
  selector: 'app-transactions-page',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, InputTextModule, DatePipe, DecimalPipe],
  templateUrl: './transactions-page.component.html',
  styleUrls: ['./transactions-page.component.scss'],
})
export class TransactionsPageComponent {
  transactions: TransactionDto[] = [];
  loading = false;
  sparplanId = '';

  depositAmount = 0;
  feePending = false;

  // Close flow
  closeInfo: any = null; // info from /close/request
  bankAccount = '';
  sessionToken = '';

  constructor(private transactionsService: TransactionsService) {}

  loadTransactions(): void {
    if (!this.sparplanId) {
      alert('Bitte Sparplan-ID eingeben.');
      return;
    }

    this.loading = true;
    this.transactionsService.getTransactions(this.sparplanId).subscribe({
      next: (data) => {
        this.transactions = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Fehler beim Laden der Transaktionen', err);
        this.loading = false;
      },
    });
  }

  deposit(): void {
    const dto: TransactionDepositDto = {
      sparplanId: this.sparplanId,
      amountInCurrency: this.depositAmount,
    };

    this.transactionsService.deposit(dto).subscribe({
      next: () => {
        alert('✅ Einzahlung erfolgreich verbucht');
        this.loadTransactions();
      },
      error: (err) => alert('❌ Fehler bei Einzahlung: ' + err.message),
    });
  }

  bookFee(): void {
    const dto: TransactionFeeDto = { sparplanId: this.sparplanId };
    this.transactionsService.fee(dto).subscribe({
      next: () => {
        alert('💰 Quartalsgebühr erfolgreich abgebucht');
        this.loadTransactions();
      },
      error: (err) => alert('❌ Fehler bei Gebührenbuchung: ' + err.message),
    });
  }

  requestClose(): void {
    const dto: TransactionCloseRequestDto = { sparplanId: this.sparplanId };

    this.transactionsService.requestClose(dto).subscribe({
      next: (info) => {
        this.closeInfo = info;
        this.sessionToken = info.sessionToken;

        alert(
          `✅ Beendigungsanfrage erhalten!\n\n` +
          `Metall: ${info.metal}\n` +
          `Barren: ${info.bars}\n` +
          `Auszahlung: ${info.estimatedPayout} EUR\n\n` +
          `${info.message}`
        );
      },
      error: (err) => {
        console.error('❌ Fehler bei Beendigungsanfrage', err);
        alert('❌ Fehler bei Beendigungsanfrage: ' + err.message);
      },
    });
  }


  confirmClose(): void {
    if (!this.bankAccount) {
      alert('Bitte Bankverbindung eingeben.');
      return;
    }

    const dto: TransactionCloseConfirmDto = {
      sparplanId: this.sparplanId,
      bankAccount: this.bankAccount,
      sessionToken: this.sessionToken,
    };

    this.transactionsService.confirmClose(dto).subscribe({
      next: (res: any) => {
        alert(`✅ Sparplan geschlossen. Betrag ${res.paidOut} EUR an ${res.bankAccount}`);
        this.closeInfo = null;
        this.bankAccount = '';
        this.sessionToken = '';
        this.loadTransactions();
      },
      error: (err) => alert('❌ Fehler bei Bestätigung: ' + err.message),
    });
  }
}
