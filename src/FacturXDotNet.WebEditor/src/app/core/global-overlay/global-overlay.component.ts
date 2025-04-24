import { Component, inject, signal } from '@angular/core';
import { GlobalOverlayService } from './global-overlay.service';

@Component({
  selector: 'app-global-overlay',
  imports: [],
  template: `
    <div
      class="backdrop position-absolute start-0 end-0 top-0 bottom-0 d-flex flex-column justify-content-center align-items-center pe-none"
      [class.d-none]="!enabled()"
      style="z-index: 9999"
    >
      <div class="card">
        <div class="card-body d-flex flex-column align-items-center">
          <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
          @if (message(); as message) {
            {{ message }}
          }
        </div>
      </div>
    </div>
  `,
  styles: `
    .backdrop {
      background-color: rgba(0, 0, 0, 0.5);
    }
  `,
})
export class GlobalOverlayComponent {
  private globalOverlayService = inject(GlobalOverlayService);
  protected enabled = this.globalOverlayService.enabled;
  protected message = this.globalOverlayService.message;
}
