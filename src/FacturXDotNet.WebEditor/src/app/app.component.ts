import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToasterComponent } from './core/toasts/toaster.component';
import { ImportFileComponent } from './core/import-file/import-file.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToasterComponent, ImportFileComponent],
  template: `
    <router-outlet />
    <app-toaster></app-toaster>
    <app-import-file></app-import-file>
  `,
  styles: [],
})
export class AppComponent {}
