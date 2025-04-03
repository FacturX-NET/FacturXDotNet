import { Component, inject } from '@angular/core';
import { EditorStateService } from '../../services/editor-state.service';

@Component({
  selector: 'app-editor-file-menu',
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
})
export class EditorFileMenuComponent {
  private editorStateService = inject(EditorStateService);

  protected async createNewFacturXDocument() {
    await this.editorStateService.clear();
  }
}
