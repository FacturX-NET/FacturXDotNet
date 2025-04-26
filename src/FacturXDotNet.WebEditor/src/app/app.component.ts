import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToasterComponent } from './core/toasts/toaster.component';
import { ImportFileComponent } from './core/import-file/import-file.component';
import { NgbTooltipConfigComponent } from './core/ng-bootstrap/ngb-tooltip-config.component';
import * as pdf from 'pdfjs-dist';
import { PlatformLocation } from '@angular/common';
import { GlobalOverlayComponent } from './core/global-overlay/global-overlay.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToasterComponent, ImportFileComponent, NgbTooltipConfigComponent, GlobalOverlayComponent],
  template: `
    <router-outlet />
    <app-toaster />
    <app-import-file />
    <app-global-overlay />

    <app-ngb-tooltip-config />
  `,
  styles: [],
})
export class AppComponent {
  constructor() {
    const platformLocation = inject(PlatformLocation);
    pdf.GlobalWorkerOptions.workerSrc = trimEnd(platformLocation.getBaseHrefFromDOM(), '/') + '/pdf.worker.min.mjs';
  }
}

function trimEnd(str: string, end: string) {
  return str.endsWith(end) ? str.substring(0, str.length - end.length) : str;
}
