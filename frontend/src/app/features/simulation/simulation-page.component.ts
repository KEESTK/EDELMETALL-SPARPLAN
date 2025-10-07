import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChartModule } from 'primeng/chart';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker'; 

import { SimulationService } from './simulation.service';
import { SimulationResult } from '../../types/models';
import { MetalType } from '../../types/enums';

@Component({
  selector: 'app-simulation-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ChartModule,
    ButtonModule,
    SelectModule,
    InputTextModule,
    DatePickerModule
  ],
  templateUrl: './simulation-page.component.html',
  styleUrls: ['./simulation-page.component.scss']
})
export class SimulationPageComponent {
  metals = [
    { label: 'Gold', value: MetalType.Gold },
    { label: 'Silber', value: MetalType.Silver }
  ];

  selectedMetal: MetalType = MetalType.Gold;
  monthlyRate: number = 200;
  from: Date = new Date(new Date().setFullYear(new Date().getFullYear() - 3));
  to: Date = new Date();

  results: SimulationResult[] = [];
  loading = false;

  chartData: any;
  chartOptions: any;

  constructor(private simulationService: SimulationService) {}

  simulate() {
    this.loading = true;

    const fromStr = this.from.toISOString();
    const toStr = this.to.toISOString();

    this.simulationService.simulate(this.selectedMetal, this.monthlyRate, fromStr, toStr)
      .subscribe({
        next: (data) => {
          this.results = data;
          this.prepareChart();
          this.loading = false;
        },
        error: (err) => {
          console.error('Simulation failed', err);
          this.loading = false;
        }
      });
  }

  prepareChart() {
    const labels = this.results.map(r => new Date(r.date));
    const deposits = this.results.map(r => r.deposits);
    const bars = this.results.map(r => r.bars);
    const market = this.results.map(r => r.marketValue);
    const profit = this.results.map(r => r.profitLoss);

    this.chartData = {
      labels,
      datasets: [
        {
          label: '💰 Einzahlungen (Gesamt)',
          data: deposits,
          borderColor: '#42A5F5',
          backgroundColor: 'rgba(66, 165, 245, 0.1)',
          fill: true,
          pointRadius: 2,
          pointHoverRadius: 5,
          tension: 0.25
        },
        {
          label: '🏅 Bestände (Barren)',
          data: bars,
          borderColor: '#AB47BC',
          backgroundColor: 'rgba(171, 71, 188, 0.1)',
          fill: true,
          pointRadius: 2,
          pointHoverRadius: 5,
          tension: 0.25
        },
        {
          label: '📈 Marktwert (€)',
          data: market,
          borderColor: '#66BB6A',
          backgroundColor: 'rgba(102, 187, 106, 0.1)',
          fill: true,
          pointRadius: 2,
          pointHoverRadius: 5,
          tension: 0.25
        },
        {
          label: '📉 Gewinn / Verlust (€)',
          data: profit,
          borderColor: '#EF5350',
          backgroundColor: 'rgba(239, 83, 80, 0.1)',
          fill: true,
          pointRadius: 2,
          pointHoverRadius: 5,
          tension: 0.25
        }
      ]
    };

    this.chartOptions = {
      responsive: true,
      maintainAspectRatio: false, // ✅ Let container define the size
      interaction: { mode: 'index', intersect: false },
      plugins: {
        legend: {
          position: 'bottom',
          labels: { usePointStyle: true, padding: 15, font: { size: 13 } }
        },
        title: {
          display: true,
          text: '📊 Simulation der Wertentwicklung',
          color: '#333',
          font: { size: 16, weight: 'bold' },
          padding: { top: 10, bottom: 20 }
        },
        tooltip: { enabled: true }
      },
      scales: {
        x: {
          type: 'time',
          time: { unit: 'month', displayFormats: { month: 'MMM yyyy' } },
          title: { display: true, text: 'Zeitraum' },
          grid: { color: '#eee' }
        },
        y: {
          title: { display: true, text: 'Wert (€)' },
          grid: { color: '#eee' },
          beginAtZero: true,
          grace: '10%',
        }
      }
    };
  }
}
