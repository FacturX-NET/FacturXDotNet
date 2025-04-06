import { Component, computed, inject } from '@angular/core';
import { HealthApi } from '../health.api';
import { rxResource } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-api-server-status',
  imports: [],
  template: `
    @if (health.isLoading()) {
      <div class="placeholder-glow">
        <span class="placeholder placeholder" style="width:100px"></span>
      </div>
    } @else {
      <div class="text-truncate" style="max-width: 150px">
        <i class="bi bi-{{ healthIcon() }}"></i>
        {{ health.value() ?? 'Unreachable' }}
      </div>
    }
  `,
  styles: ``,
})
export class ApiServerStatusComponent {
  reachable = computed(() => this.health.value() === 'Healthy');
  status = computed(() => this.health.value() ?? 'Unreachable');
  loading = computed(() => this.health.isLoading());

  private healthApi = inject(HealthApi);
  protected health = rxResource({ loader: () => this.healthApi.getHealth() });
  protected healthIcon = computed(() => computeHealthIcon(this.health.value()));
}

function computeHealthIcon(value: string | undefined) {
  switch (value) {
    case 'Healthy':
      return 'check-lg';
    case 'Degraded':
      return 'exclamation-triangle-fill';
    case 'Unhealthy':
    default:
      return 'x-circle-fill';
  }
}
