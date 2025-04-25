import { Component, computed, input, linkedSignal, signal, Signal, untracked, WritableSignal } from '@angular/core';
import semver from 'semver/preload';
import { NgTemplateOutlet } from '@angular/common';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import MiniSearch from 'minisearch';
import { HighlightTextPipe } from '../../core/highlight-text/highlight-text.pipe';

const UnknownLicenseName = 'N/A';

@Component({
  selector: 'app-about-licenses',
  imports: [NgTemplateOutlet, NgbTooltip, FormsModule, HighlightTextPipe],
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

    <div class="d-flex gap-2 pt-3">
      <div>
        <label class="col-form-label col-form-label-sm">
          <i class="bi bi-search"></i>
        </label>
      </div>
      <div class="flex-grow-1">
        <input class="form-control form-control-sm" placeholder="Search..." [ngModel]="searchTerm()" (ngModelChange)="search($event)" />
      </div>
    </div>

    <div class="text-body-tertiary small text-end">{{ searchResult().packagesCount }} result(s)</div>

    <ng-container [ngTemplateOutlet]="dependenciesTpl" [ngTemplateOutletContext]="{ $implicit: searchResult() }"></ng-container>

    <ng-template #dependenciesTpl let-record>
      <div class="list-group list-group-flush">
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
                  <a class="pe-1" [href]="package_.latest.link" [innerHtml]="package_.latest.name | highlightText: package_.latest.terms"></a>

                  @switch (package_.versions.length) {
                    @case (0) {}
                    @case (1) {
                      <span class="fw-semibold" [innerHtml]="'v' + package_.versions[0] | highlightText: package_.latest.terms"></span>
                    }
                    @default {
                      @for (version of package_.versions; track version) {
                        @if ($last) {
                          <span class="fw-semibold" [innerHtml]="'v' + version | highlightText: package_.latest.terms"></span>
                        } @else {
                          <span class="fw-semibold" [innerHtml]="'v' + version | highlightText: package_.latest.terms"></span>,
                        }
                      }
                    }
                  }

                  @if (package_.latest.author) {
                    -
                    <span class="text-body-secondary fw-semibold" [innerHtml]="package_.latest.author | highlightText: package_.latest.terms"> </span>
                  }

                  @if (package_.latest.description) {
                    <div class="text-body-secondary" [innerHtml]="package_.latest.description | highlightText: package_.latest.terms"></div>
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
    source: this.packages,
    computation: (source, previous) => {
      const licenses = new Set(source.map((p) => p.license ?? UnknownLicenseName));

      const newValue = previous !== undefined ? { ...previous.value } : {};
      for (const license of licenses) {
        newValue[license] = newValue[license] ?? signal(false);
      }
      return newValue;
    },
  });

  private miniSearch = computed(() => {
    const packages = this.packages().map((p, i) => ({ ...p, index: i }));
    const result = new MiniSearch({
      idField: 'index',
      fields: ['name', 'description', 'author', 'version', 'license', 'link'],
      searchOptions: { prefix: true, fuzzy: true },
    });
    result.addAll(packages);
    return result;
  });
  protected searchTerm = signal<string | undefined>(undefined);
  protected searchResult = computed(() => {
    const miniSearch = this.miniSearch();
    const allPackages = untracked(() => this.packages());

    let packages: (Package & { terms: string[] })[];
    const searchTerm = this.searchTerm();
    if (searchTerm === undefined) {
      packages = allPackages.map((p) => ({ ...p, terms: [] }));
    } else {
      packages = miniSearch.search(searchTerm).map((r) => ({ ...allPackages[r.id], terms: r.terms }));
    }

    const activeTab = this.activeTab();
    if (activeTab === 'direct') {
      packages = packages.filter((p) => p.direct);
    }

    return groupPackages(packages);
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

  protected search(term: string | undefined) {
    if (term === undefined || term === '') {
      this.searchTerm.set(undefined);
      return;
    }

    this.searchTerm.set(term);
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

interface GroupedPackages<TPackage extends Package = Package> {
  packagesCount: number;
  licenses: {
    license: string;
    packages: {
      name: string;
      versions: string[];
      latest: TPackage;
      packages: TPackage[];
    }[];
  }[];
}

function groupPackages<TPackage extends Package>(packages: TPackage[], keepLatestOnly: boolean = false): GroupedPackages<TPackage> {
  const licenses: Record<string, Record<string, TPackage[]>> = {};

  for (const p of packages) {
    const license = p.license ?? UnknownLicenseName;

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
    packagesCount:
      Object.values(licenses).length === 0
        ? 0
        : Object.values(licenses)
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
