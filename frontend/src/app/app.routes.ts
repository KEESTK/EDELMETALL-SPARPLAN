import { Routes } from '@angular/router';
import { DepotsPageComponent } from './features/depots/depots-page.component';
import { SparplaenePageComponent } from './features/sparplaene/sparplaene-page.component';
import { SimulationPageComponent } from './features/simulation/simulation-page.component';
import { TransactionsPageComponent } from './features/transactions/transactions-page.component';

export const routes: Routes = [
  { path: '', redirectTo: 'simulation', pathMatch: 'full' },
  { path: 'depots', component: DepotsPageComponent },
  { path: 'sparplaene', component: SparplaenePageComponent,  pathMatch: 'full'},
  { path: 'simulation', component: SimulationPageComponent,  pathMatch: 'full' },
  { path: 'transactions', component: TransactionsPageComponent,  pathMatch: 'full' }
];
