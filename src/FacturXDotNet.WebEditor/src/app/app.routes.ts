import { Routes } from '@angular/router';
import { routes as editorRoutes } from './features/editor/editor.routes';

export const routes: Routes = [
  {
    path: 'about',
    loadChildren: () => import('./features/about/about.routes').then((m) => m.routes),
  },
  ...editorRoutes,
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: '/',
  },
];
