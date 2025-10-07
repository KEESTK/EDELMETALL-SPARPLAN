import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterModule, MenubarModule],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  items: MenuItem[] = [];

  ngOnInit() {
    this.items = [
      { label: '🏠 Home', routerLink: '/' },
      { label: '📦 Depots', routerLink: '/depots' },
      { label: '💰 Sparpläne', routerLink: '/sparplaene' },
      { label: '📈 Simulation', routerLink: '/simulation' },
      { label: '💳 Transactions', routerLink: '/transactions' }
    ];
  }
}
