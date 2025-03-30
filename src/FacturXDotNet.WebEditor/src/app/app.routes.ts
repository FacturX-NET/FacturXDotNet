import {Routes} from '@angular/router';

export const routes: Routes = [
  {
    path: 'welcome',
    loadChildren: () => import('./features/welcome/welcome.routes').then(m => m.routes),
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'welcome',
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: '/',
  },
];
