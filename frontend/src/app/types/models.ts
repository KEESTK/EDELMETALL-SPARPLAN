import { MetalType } from './enums';

export interface Sparplan {
  id: string;
  metal: MetalType;
  monthlyRate: number;
  startDate: string;
  isActive: boolean;
  balanceInBars: number;
  transactions: any[];
}

export interface DepotDto {
  id: string;
  totalValue?: number;
  sparplaene?: SparplanDto[];
}

export interface SparplanDto {
  id: string;
  metal: MetalType;
  monthlyRate: number;
  startDate: string;
}

export interface SparplanCreateDto {
  depotId: string;        
  metal: MetalType;
  monthlyRate: number;
}

export interface TransactionDepositDto {
  sparplanId: string;
  amountInCurrency: number; // backend expects AmountInCurrency
}

export interface TransactionFeeDto {
  sparplanId: string;
}

export interface TransactionCloseRequestDto {
  sparplanId: string;
}

export interface TransactionCloseConfirmDto {
  sparplanId: string;
  bankAccount: string;
  sessionToken: string;
}

// Transaction model for displaying transactions in the table
export interface TransactionDto {
  id: string;
  type: string; // e.g., "Deposit", "Fee", "Close"
  amountInCurrency: number;
  amountInBars: number;
  date: string; // ISO timestamp
  metal: string;
  sparplanId: string;
}


export interface SimulationResult {
  date: string;       // corresponds to C# DateTime
  deposits: number;   // kumulierte Einzahlungen
  bars: number;       // Bestände in Barren
  marketValue: number; // Marktwert in €
  profitLoss: number; // Gewinn/Verlust
}


