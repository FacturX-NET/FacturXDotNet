import { Component, computed, input, linkedSignal, signal, Signal, WritableSignal } from '@angular/core';
import semver from 'semver/preload';
import { NgTemplateOutlet } from '@angular/common';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-about-licenses',
  imports: [NgTemplateOutlet, NgbTooltip],
  template: `
    <div class="d-flex align-items-center justify-content-between gap-4">
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
      <div>
        <button class="btn" (click)="collapseAll()">
          <i class="bi bi-chevron-contract" ngbTooltip="Collapse all"></i>
        </button>
        <button class="btn" (click)="expandAll()">
          <i class="bi bi-chevron-expand" ngbTooltip="Expand all"></i>
        </button>
      </div>
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
            <a role="button" (click)="hideLicense()[license.license].set(!hideLicense()[license.license]())">
              @if (hideLicense()[license.license]()) {
                <i class="bi bi-chevron-right"></i>
              } @else {
                <i class="bi bi-chevron-down"></i>
              }
              <span class="fw-bold"> {{ license.license }} </span> ({{ license.packages.length }})
            </a>
            <ul [class.d-none]="hideLicense()[license.license]()">
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

                  @if (package_.latest.author) {
                    -
                    <span class="text-body-secondary fw-semibold">
                      {{ package_.latest.author }}
                    </span>
                  }

                  @if (package_.latest.description) {
                    <div class="text-body-secondary">
                      {{ package_.latest.description }}
                    </div>
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
  protected hideLicense: Signal<Record<string, WritableSignal<boolean>>> = linkedSignal({
    source: this.allDependenciesRecord,
    computation: (source, previous) => {
      const newValue = previous !== undefined ? { ...previous.value } : {};
      for (const license of source.licenses) {
        newValue[license.license] = newValue[license.license] ?? signal(false);
      }
      return newValue;
    },
  });

  protected collapseAll() {
    const hideLicense = this.hideLicense();
    for (const value of Object.values(hideLicense)) {
      value.set(true);
    }
  }

  protected expandAll() {
    const hideLicense = this.hideLicense();
    for (const value of Object.values(hideLicense)) {
      value.set(false);
    }
  }
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
