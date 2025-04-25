import { Component, computed, input, signal, Signal } from '@angular/core';
import semver from 'semver/preload';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-about-licenses',
  imports: [NgTemplateOutlet],
  template: `
    <div class="d-flex align-items-center gap-4">
      <ul class="nav nav-underline">
        <li class="nav-item">
          <button class="nav-link text-truncate" [class.active]="activeTab() === 'direct'" (click)="activeTab.set('direct')">
            Direct dependencies ({{ directDependenciesRecord().packagesCount }})
          </button>
        </li>
        <li class="nav-item">
          <button class="nav-link text-truncate" [class.active]="activeTab() === 'all'" (click)="activeTab.set('all')">
            All dependencies ({{ allDependenciesRecord().packagesCount }})
          </button>
        </li>
      </ul>
    </div>

    <div [class.d-none]="activeTab() !== 'direct'">
      <ng-container [ngTemplateOutlet]="dependenciesTpl" [ngTemplateOutletContext]="{ $implicit: directDependenciesRecord() }"></ng-container>
    </div>
    <div [class.d-none]="activeTab() !== 'all'">
      <ng-container [ngTemplateOutlet]="dependenciesTpl" [ngTemplateOutletContext]="{ $implicit: allDependenciesRecord() }"></ng-container>
    </div>

    <ng-template #dependenciesTpl let-record>
      <div class="list-group list-group-flush pt-2">
        @for (license of record.licenses; track license.license) {
          <div class="list-group-item">
            <span class="fw-bold"> {{ license.license }} </span> ({{ license.packages.length }})
            <ul>
              @for (package_ of license.packages; track package_.name) {
                <li>
                  <a [href]="package_.latest.link">{{ package_.latest.name }}</a>

                  @switch (package_.versions.length) {
                    @case (0) {}
                    @case (1) {
                      <span class="fw-semibold"> v{{ package_.versions[0] }} </span>
                    }
                    @default {
                      @for (version of package_.versions; track version) {
                        @if ($last) {
                          <span class="fw-semibold"> v{{ version }} </span>
                        } @else {
                          <span class="fw-semibold"> v{{ version }}</span
                          >,
                        }
                      }
                    }
                  }

                  @if (package_.latest.description) {
                    -
                    <span class="text-body-secondary">
                      {{ package_.latest.description }}
                    </span>
                  }
                  @if (package_.latest.author) {
                    -
                    <span class="text-body-secondary fw-semibold">
                      {{ package_.latest.author }}
                    </span>
                  }
                </li>
              }
            </ul>
          </div>
        }
      </div>
    </ng-template>
  `,
})
export class AboutLicensesComponent {
  packages = input.required<Package[]>();

  protected directDependenciesRecord: Signal<GroupedPackages> = computed(() =>
    groupPackages(
      this.packages().filter((p) => p.direct),
      true,
    ),
  );
  protected allDependenciesRecord: Signal<GroupedPackages> = computed(() => groupPackages(this.packages()));

  protected activeTab = signal<'direct' | 'all'>('direct');
}

export interface Package {
  name: string;
  description?: string;
  author?: string;
  version: string;
  license?: string;
  link?: string;
  direct: boolean;
}

interface GroupedPackages {
  packagesCount: number;
  licenses: {
    license: string;
    packages: {
      name: string;
      versions: string[];
      latest: Package;
      packages: Package[];
    }[];
  }[];
}

function groupPackages(packages: Package[], keepLatestOnly: boolean = false): GroupedPackages {
  const licenses: Record<string, Record<string, Package[]>> = {};

  for (const p of packages) {
    const license = p.license ?? 'N/A';

    if (!licenses[license]) {
      licenses[license] = {};
    }

    if (!licenses[license][p.name]) {
      licenses[license][p.name] = [];
    }

    if (keepLatestOnly) {
      if (licenses[license][p.name].every((x) => semver.lt(x.version, p.version))) {
        licenses[license][p.name] = [p];
      }
    } else {
      licenses[license][p.name].push(p);
    }
  }

  return {
    packagesCount: Object.values(licenses)
      .map((l) => Object.keys(l).length)
      .reduce((a, b) => a + b),
    licenses: Object.entries(licenses).map(([license, packages]) => ({
      license,
      packages: Object.entries(packages)
        .map(([name, packages]) => ({
          name,
          versions: [...new Set(packages.map((p) => p.version).filter((v) => v !== undefined && v !== null && v != ''))].sort((a, b) => -semver.compare(a, b)),
          latest: packages.sort((a, b) => -semver.compare(a.version, b.version))[0],
          packages,
        }))
        .sort((a, b) => a.name.localeCompare(b.name)),
    })),
  };
}
