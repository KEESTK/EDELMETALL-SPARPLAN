import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DepotsService } from './depots.service';
import { DepotDto, SparplanCreateDto } from '../../types/models';
import { MetalType } from '../../types/enums';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-depots-page',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonModule, TableModule, ToastModule],
  providers: [MessageService],
  templateUrl: './depots-page.component.html',
  styleUrls: ['./depots-page.component.scss'],
})
export class DepotsPageComponent implements OnInit {
  depots: DepotDto[] = [];
  loading = false;

  metalOptions = Object.values(MetalType).map(m => ({
    label: m,
    value: m
  }));

  lastCreatedDepotId: string | null = null;
  lastAddedSparplanId: string | null = null;
  lastAddedDepotId: string | null = null; 


  constructor(
    private depotsService: DepotsService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.depotsService.getAll().subscribe({
      next: (data) => {
        this.depots = data.map(d => ({
          ...d,
          selectedMetal: MetalType.Gold,
          selectedRate: 100,
        }));
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading depots', err);
        this.loading = false;
      },
    });
  }


  createDepot(): void {
    console.log('📦 Creating depot...');
    this.depotsService.create().subscribe({
      next: (depot) => {
        // After creating depot
        this.lastCreatedDepotId = depot.id;
        console.log(`✅ Created Depot: ${this.lastCreatedDepotId}`);
        // After creating depot

        this.messageService.add({
          severity: 'success',
          summary: 'Depot erstellt',
          detail: `Neues Depot mit ID ${this.lastCreatedDepotId} wurde erfolgreich erstellt.`,
        });

        this.load();
      },
      error: (err) => {
        console.error('❌ Error creating depot', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Fehler',
          detail: 'Depot konnte nicht erstellt werden.',
        });
      },
    });
  }

  addSparplan(depotId: string, metal: MetalType, monthlyRate: number): void {
    const dto: SparplanCreateDto = {
      metal,
      monthlyRate,
      depotId,
    };

    console.log(`➕ Adding Sparplan to Depot ${depotId}: ${metal}, ${monthlyRate}€/Monat`);
    this.depotsService.addSparplan(depotId, dto).subscribe({
      next: (updatedDepot) => {
        const lastSparplan = updatedDepot.sparplaene?.at(-1);
        this.lastAddedSparplanId = lastSparplan?.id ?? 'unbekannt';
        this.lastAddedDepotId = updatedDepot.id;

        this.messageService.add({
          severity: 'success',
          summary: 'Sparplan hinzugefügt',
          detail: `Sparplan ${this.lastAddedSparplanId} wurde Depot ${updatedDepot.id} hinzugefügt.`,
        });

        this.load();
      },
      error: (err) => {
        console.error('❌ Error adding Sparplan', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Fehler',
          detail: 'Sparplan konnte nicht hinzugefügt werden.',
        });
      },
    });
  }

}
