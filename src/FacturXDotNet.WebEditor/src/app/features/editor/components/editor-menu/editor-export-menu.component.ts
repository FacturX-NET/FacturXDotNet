import { Component } from '@angular/core';

@Component({
  selector: 'app-editor-export-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">Export</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" href="#">Download FacturX document</a>
          <a class="dropdown-item" href="#">Download Cross-Industry Invoice XML file</a>
          <a class="dropdown-item" href="#">Download PDF file</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorExportMenuComponent {}
