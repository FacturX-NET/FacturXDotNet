import { Component } from '@angular/core';

@Component({
  selector: 'app-editor-import-menu',
  imports: [],
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">Import</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" href="#">Import Cross-Industry Invoice data</a>
          <a class="dropdown-item" href="#">Import PDF image</a>
        </li>
      </ul>
    </li>
  `,
  styles: ``,
})
export class EditorImportMenuComponent {}
