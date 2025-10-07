import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

import { SparplaeneService } from './sparplaene.service';
import { SparplanDto, SparplanCreateDto } from '../../types/models';
import { MetalType } from '../../types/enums';

@Component({
  selector: 'app-sparplaene-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DatePipe,
    DecimalPipe,
    TableModule,
    ToolbarModule,
    ButtonModule,
    SelectModule,
    InputTextModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './sparplaene-page.component.html',
  styleUrls: ['./sparplaene-page.component.scss']
})
export class SparplaenePageComponent implements OnInit {
  sparplaene: SparplanDto[] = [];
  loading = true;

  // form model for creating new Sparplan
  newSparplan: SparplanCreateDto = {
    depotId: '',
    metal: MetalType.Gold,
    monthlyRate: 100
  };

  // info about last created
  lastCreatedSparplanId: string | null = null;
  lastUsedDepotId: string | null = null;

  metalOptions = Object.values(MetalType).map(m => ({ label: m, value: m }));

  constructor(
    private sparplaeneService: SparplaeneService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  /** 🔹 Load all existing Sparpläne */
  load(): void {
    this.loading = true;
    this.sparplaeneService.getAll().subscribe({
      next: (data) => {
        this.sparplaene = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading Sparpläne', err);
        this.loading = false;
      }
    });
  }

  /** 🔹 Create a new Sparplan inline */
  createSparplan(): void {
    if (!this.newSparplan.depotId) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Fehlende Eingabe',
        detail: 'Bitte geben Sie eine Depot-ID an.'
      });
      return;
    }

    this.sparplaeneService.create(this.newSparplan).subscribe({
      next: (sparplan) => {
        this.lastCreatedSparplanId = sparplan.id;
        this.lastUsedDepotId = this.newSparplan.depotId;

        this.messageService.add({
          severity: 'success',
          summary: 'Sparplan erstellt',
          detail: `Sparplan ${sparplan.id} wurde Depot ${this.newSparplan.depotId} hinzugefügt.`
        });

        // Reset form for next input
        this.newSparplan = {
          depotId: '',
          metal: MetalType.Gold,
          monthlyRate: 100
        };

        this.load();
      },
      error: (err) => {
        console.error('❌ Error creating Sparplan', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Fehler',
          detail: 'Sparplan konnte nicht erstellt werden.'
        });
      }
    });
  }
}

