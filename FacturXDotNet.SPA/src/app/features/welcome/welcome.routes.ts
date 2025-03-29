import { Routes } from '@angular/router';
import { WelcomePage } from './welcome.page';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: WelcomePage,
  },
];
