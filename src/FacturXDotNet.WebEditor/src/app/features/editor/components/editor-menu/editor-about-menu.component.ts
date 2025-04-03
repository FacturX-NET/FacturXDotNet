import { Component, input } from '@angular/core';

@Component({
  selector: 'app-editor-about-menu',
  imports: [],
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">About</a>
      <ul class="dropdown-menu">
        @if (showSelfHostingMenu()) {
          <li>
            <a class="dropdown-item" href="#">Self-hosting</a>
          </li>
        }
        <li>
          <a class="dropdown-item" href="#">About FacturX.NET Web Editor</a>
        </li>
      </ul>
    </li>
  `,
  styles: ``,
})
export class EditorAboutMenuComponent {
  showSelfHostingMenu = input<boolean>(false);
}
