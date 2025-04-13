import { Component, inject } from '@angular/core';
import { NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ngb-tooltip-config',
  imports: [],
  template: ``,
  styles: ``,
})
export class NgbTooltipConfigComponent {
  constructor() {
    const config = inject(NgbTooltipConfig);

    config.container = 'body';
  }
}
