import { Component, inject } from '@angular/core';
import { CurrentCiiService } from '../../services/current-cii.service';

@Component({
  selector: 'app-editor-file-menu',
  imports: [],
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">File</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" href="#" (click)="createNewFacturXDocument()">Create blank FacturX document</a>
          <a class="dropdown-item" href="#">Open FacturX document</a>
        </li>
      </ul>
    </li>
  `,
  styles: ``,
})
export class EditorFileMenuComponent {
  private currentCiiService = inject(CurrentCiiService);

  protected createNewFacturXDocument() {
    this.currentCiiService.clear();
  }
}
