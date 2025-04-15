import { Component, inject, input } from '@angular/core';
import { EditorSavedState } from '../editor-state.service';
import { EditorSettings } from '../editor-settings.service';
import { EditorHeaderNameComponent } from './editor-header-name.component';
import { Router, RouterLink } from '@angular/router';
import { NgbNav, NgbNavItem, NgbNavLink } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-editor-left-pane-header',
  template: `
    <ul ngbNav [activeId]="route.url" class="nav-tabs ps-4">
      <li ngbNavItem="/xmp">
        <a ngbNavLink role="button" routerLink="/xmp"> <i class="bi bi-code"></i> XMP Metadata </a>
      </li>
      <li ngbNavItem="/cii">
        <a ngbNavLink role="button" routerLink="/cii">
          <i class="bi bi-code"></i>
          Cross-Industry Invoice
        </a>
      </li>
      <li ngbNavItem="/attachments">
        <a ngbNavLink role="button" routerLink="/attachments"> <i class="bi bi-paperclip"></i> Attachments ({{ state().attachments.length }}) </a>
      </li>
    </ul>
  `,
  imports: [NgbNav, NgbNavItem, NgbNavLink, RouterLink],
})
export class EditorLeftPaneHeaderComponent {
  state = input.required<EditorSavedState>();
  settings = input.required<EditorSettings>();

  protected route = inject(Router);
}
