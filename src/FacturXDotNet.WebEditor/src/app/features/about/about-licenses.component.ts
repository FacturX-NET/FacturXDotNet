import { Component, computed, input, Signal } from '@angular/core';
import { IPackageDto } from '../../core/api/api.models';

@Component({
  selector: 'app-about-licenses',
  template: `
    <div class="list-group list-group-flush">
      @for (license of licenses(); track license) {
        <div class="list-group-item">
          <span class="fw-bold"> {{ license }} </span> ({{ packagesRecord()[license].length }})
          <div>
            @for (p of packagesRecord()[license]; track p.name) {
              <div>
                <a [href]="p.link">{{ p.name }}</a>
                v{{ p.version }}
                -
                <span class="text-body-secondary">
                  {{ p.author }}
                </span>
              </div>
            }
          </div>
        </div>
      }
    </div>
  `,
})
export class AboutLicensesComponent {
  packages = input.required<IPackageDto[]>();

  protected packagesRecord: Signal<Record<string, IPackageDto[]>> = computed(() => {
    const result: Record<string, IPackageDto[]> = {};

    for (const p of this.packages()) {
      if (!result[p.license]) {
        result[p.license] = [];
      }

      result[p.license].push(p);
    }

    return result;
  });

  protected licenses: Signal<string[]> = computed(() => {
    const result = Object.keys(this.packagesRecord());
    result.sort((a, b) => a.localeCompare(b));
    return result;
  });
}
