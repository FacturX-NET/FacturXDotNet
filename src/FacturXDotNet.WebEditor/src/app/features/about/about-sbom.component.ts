import { Component, computed, input, linkedSignal, signal, Signal, untracked, WritableSignal } from '@angular/core';
import semver from 'semver/preload';
import { NgTemplateOutlet } from '@angular/common';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import MiniSearch from 'minisearch';
import { HighlightTextPipe } from '../../core/highlight-text/highlight-text.pipe';
import { Sbom } from '../../core/sbom';
import { Dependency, extractDependenciesFromSbom } from './dependency';
import { downloadBlob, downloadFile } from '../../core/utils/download-blob';
import { MarkdownComponent } from 'ngx-markdown';
import { EscapeHtmlPipe } from '../../core/escape-html/escape-html.pipe';

@Component({
  selector: 'app-about-licenses',
  imports: [NgTemplateOutlet, NgbTooltip, FormsModule, HighlightTextPipe, MarkdownComponent, EscapeHtmlPipe],
  template: `
    <h6>Dependencies</h6>
    <div class="d-flex flex-wrap align-items-center justify-content-between gap-4">
      <ul class="nav nav-underline">
        <li class="nav-item">
          <button class="nav-link text-truncate" [class.active]="activeTab() === 'direct'" (click)="activeTab.set('direct')">Direct ({{ directDependenciesCount() }})</button>
        </li>
        <li class="nav-item">
          <button class="nav-link text-truncate" [class.active]="activeTab() === 'all'" (click)="activeTab.set('all')">All ({{ allDependenciesCount() }})</button>
        </li>
      </ul>
      <div class="d-flex flex-nowrap">
        <button class="btn" (click)="collapseAll()">
          <i class="bi bi-chevron-contract" ngbTooltip="Collapse all"></i>
        </button>
        <button class="btn" (click)="expandAll()">
          <i class="bi bi-chevron-expand" ngbTooltip="Expand all"></i>
        </button>
        <button class="btn" (click)="downloadSbom()">
          <i class="bi bi-download" ngbTooltip="Download SBOM"></i>
        </button>
      </div>
    </div>

    <div class="d-flex gap-2 pt-3 ps-3">
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
            <a role="button" (click)="collapsedBlocks()[license.license].set(!collapsedBlocks()[license.license]())">
              @if (collapsedBlocks()[license.license]()) {
                <i class="bi bi-chevron-right"></i>
              } @else {
                <i class="bi bi-chevron-down"></i>
              }

              @if (license.license === '') {
                Unknown
              } @else {
                <span class="fw-bold"> {{ license.license }} </span>
              }
              ({{ license.packages.length }})
            </a>
            <ul [class.d-none]="collapsedBlocks()[license.license]()">
              @for (package_ of license.packages; track package_.name) {
                <li>
                  <a class="pe-1" [href]="package_.latest.link" [innerHtml]="package_.latest.name | escapeHtml | highlightText: package_.latest.terms"></a>

                  @switch (package_.versions.length) {
                    @case (0) {}
                    @case (1) {
                      <span class="fw-semibold" [innerHtml]="'v' + package_.versions[0] | escapeHtml | highlightText: package_.latest.terms"></span>
                    }
                    @default {
                      @for (version of package_.versions; track version) {
                        @if ($last) {
                          <span class="fw-semibold" [innerHtml]="'v' + version | escapeHtml | highlightText: package_.latest.terms"></span>
                        } @else {
                          <span class="fw-semibold" [innerHtml]="'v' + version | escapeHtml | highlightText: package_.latest.terms"></span>,
                        }
                      }
                    }
                  }

                  @if (package_.latest.author) {
                    -
                    <span class="text-body-secondary fw-semibold" [innerHtml]="package_.latest.author | escapeHtml | highlightText: package_.latest.terms"> </span>
                  }

                  @if (package_.latest.description) {
                    <p class="text-body-secondary">
                      <markdown ngPreserveWhitespaces>
                        {{ package_.latest.description | escapeHtml | highlightText: package_.latest.terms }}
                      </markdown>
                    </p>
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
export class AboutSbomComponent {
  sbom = input.required<Sbom>();
  sbomName = input<string>();

  protected dependencies = computed(() => extractDependenciesFromSbom(this.sbom()));

  protected directDependenciesCount = computed(() => {
    const names = new Set(
      this.dependencies()
        .filter((d) => d.direct)
        .map((d) => d.name),
    );
    return names.size;
  });

  protected allDependenciesCount = computed(() => {
    const names = new Set(this.dependencies().map((d) => d.name));
    return names.size;
  });

  protected activeTab = signal<'direct' | 'all'>('direct');

  private miniSearch = computed(() => {
    const packages = this.dependencies().map((p, i) => ({ ...p, index: i }));
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
    const allPackages = untracked(() => this.dependencies());

    let packages: (Dependency & { terms: string[] })[];
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

  protected collapsedBlocks: Signal<Record<string, WritableSignal<boolean>>> = linkedSignal({
    source: this.dependencies,
    computation: (source, previous) => {
      const licenses = new Set(source.map((p) => licenseNameOrDefault(p.license)));

      const newValue = previous !== undefined ? { ...previous.value } : {};
      for (const license of licenses) {
        newValue[license] = newValue[license] ?? signal(false);
      }
      return newValue;
    },
  });

  protected collapseAll() {
    const collapsedBlocks = this.collapsedBlocks();
    for (const value of Object.values(collapsedBlocks)) {
      value.set(true);
    }
  }

  protected expandAll() {
    const collapsedBlocks = this.collapsedBlocks();
    for (const value of Object.values(collapsedBlocks)) {
      value.set(false);
    }
  }

  protected downloadSbom() {
    const sbom = this.sbom();
    const name = this.sbomName();
    const serialized = JSON.stringify(sbom);
    const file = new File([serialized], name ?? 'sbom.json', { type: 'application/json' });
    downloadFile(file);
  }

  protected search(term: string | undefined) {
    if (term === undefined || term === '') {
      this.searchTerm.set(undefined);
      return;
    }

    this.searchTerm.set(term);
  }
}

interface DependenciesGroupedByLicensesThenName<TPackage extends Dependency = Dependency> {
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

function groupPackages<TPackage extends Dependency>(packages: TPackage[], keepLatestOnly: boolean = false): DependenciesGroupedByLicensesThenName<TPackage> {
  const licenses: Record<string, Record<string, TPackage[]>> = {};

  for (const p of packages) {
    const license = licenseNameOrDefault(p.license);

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
    licenses: Object.entries(licenses)
      .map(([license, packages]) => ({
        license,
        packages: Object.entries(packages)
          .map(([name, packages]) => ({
            name,
            versions: [...new Set(packages.map((p) => p.version).filter((v) => v !== undefined && v !== null && v != ''))].sort((a, b) => -semver.compare(a, b)),
            latest: packages.sort((a, b) => -semver.compare(a.version, b.version))[0],
            packages,
          }))
          .sort((a, b) => a.name.localeCompare(b.name)),
      }))
      .sort((a, b) => (a.license === '' && b.license === '' ? 0 : a.license === '' ? 1 : b.license === '' ? -1 : a.license.localeCompare(b.license))),
  };
}

function licenseNameOrDefault(name: string | undefined): string {
  return name === undefined || name === '' ? '' : name;
}
