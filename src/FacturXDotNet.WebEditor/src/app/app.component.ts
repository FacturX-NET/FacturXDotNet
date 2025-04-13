import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToasterComponent } from './core/toasts/toaster.component';
import { ImportFileComponent } from './core/import-file/import-file.component';
import { NgbTooltipConfigComponent } from './core/ng-bootstrap/ngb-tooltip-config.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToasterComponent, ImportFileComponent, NgbTooltipConfigComponent],
  template: `
    <router-outlet />
    <app-toaster></app-toaster>
    <app-import-file></app-import-file>

    <app-ngb-tooltip-config></app-ngb-tooltip-config>
  `,
  styles: [],
})
export class AppComponent {}
