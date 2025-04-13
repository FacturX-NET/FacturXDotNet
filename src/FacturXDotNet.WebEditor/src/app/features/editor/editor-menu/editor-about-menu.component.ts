import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-editor-about-menu',
  template: `
    <li class="nav-item" ngbDropdown>
      <button id="editor-about-menu" class="nav-link px-4 text-light" ngbDropdownToggle>About</button>
      <div ngbDropdownMenu aria-labelledby="editor-about-menu">
        @if (showSelfHostingMenu()) {
          <button ngbDropdownItem>Self-hosting</button>
        }
        <button routerLink="/about" ngbDropdownItem>About FacturX.NET Web Editor</button>
      </div>
    </li>
  `,
  imports: [RouterLink, NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownItem],
})
export class EditorAboutMenuComponent {
  showSelfHostingMenu = input<boolean>(false);
}
