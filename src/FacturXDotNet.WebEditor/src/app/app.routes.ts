import { Routes } from '@angular/router';
import { routes as editorRoutes } from './features/editor/editor.routes';

export const routes: Routes = [
  ...editorRoutes,
  {
    path: 'about',
    loadChildren: () => import('./features/about/about.routes').then((m) => m.routes),
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: '/',
  },
];
