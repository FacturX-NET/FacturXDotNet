import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToasterComponent } from './core/toasts/toaster.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToasterComponent],
  template: ` <router-outlet />
    <app-toaster></app-toaster>`,
  styles: [],
})
export class AppComponent {}
