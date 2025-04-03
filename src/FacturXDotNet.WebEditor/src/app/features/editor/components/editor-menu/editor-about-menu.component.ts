import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-editor-about-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">About</a>
      <ul class="dropdown-menu">
        @if (showSelfHostingMenu()) {
          <li>
            <a class="dropdown-item" href="javascript:void 0;">Self-hosting</a>
          </li>
        }
        <li>
          <a class="dropdown-item" href="javascript:void 0;" routerLink="/about">About FacturX.NET Web Editor</a>
        </li>
      </ul>
    </li>
  `,
  imports: [RouterLink],
})
export class EditorAboutMenuComponent {
  showSelfHostingMenu = input<boolean>(false);
}
