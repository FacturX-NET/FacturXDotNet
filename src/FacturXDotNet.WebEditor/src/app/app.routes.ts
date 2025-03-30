import {Routes} from '@angular/router';

export const routes: Routes = [
  {
    path: 'editor',
    loadChildren: () => import('./features/editor/editor.routes').then(m => m.routes),
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'editor',
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: '/',
  },
];
