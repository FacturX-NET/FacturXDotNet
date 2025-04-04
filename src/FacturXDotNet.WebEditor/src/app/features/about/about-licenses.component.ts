import { Component } from '@angular/core';
import licenses from '../../../licenses/licenses.json';

@Component({
  selector: 'app-about-licenses',
  template: `
    <div class="list-group">
      @for (license of licenses; track license) {
        <div class="list-group-item">
          <span class="fw-bold"> {{ license }} </span> ({{ packages[license].length }})
          <div>
            @for (p of packages[license]; track p.name) {
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
  protected licenses: string[];
  protected packages: Record<string, Package[]>;

  constructor() {
    const licensesSet = new Set<string>();
    this.packages = {};

    for (const license of licenses) {
      if (!this.packages[license.licenseType]) {
        this.packages[license.licenseType] = [];
      }

      licensesSet.add(license.licenseType);
      this.packages[license.licenseType].push({ name: license.name, author: license.author, version: license.installedVersion, license: license.licenseType, link: license.link });
    }

    this.licenses = [...licensesSet];
    this.licenses.sort((a, b) => a.localeCompare(b));
  }
}

interface Package {
  name: string;
  author: string;
  version: string;
  license: string;
  link: string;
}
