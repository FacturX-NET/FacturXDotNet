import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-editor-about-menu',
  imports: [RouterLink, NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem],
  template: `
    <li class="nav-item" ngbDropdown>
      <button id="editor-about-menu" class="nav-link px-4 text-light" ngbDropdownToggle>About</button>
      <div ngbDropdownMenu aria-labelledby="editor-about-menu">
        @if (documentationUrl !== undefined) {
          <a [href]="documentationUrl" target="_blank" ngbDropdownItem>Documentation</a>
        }
        @if (showSelfHostingMenu()) {
          <button ngbDropdownItem>Self-hosting</button>
        }
        <button routerLink="/about" ngbDropdownItem>About FacturX.NET Web Editor</button>
      </div>
    </li>
  `,
})
export class EditorAboutMenuComponent {
  showSelfHostingMenu = input<boolean>(false);

  protected documentationUrl: string | undefined = environment.documentationUrl;
}
