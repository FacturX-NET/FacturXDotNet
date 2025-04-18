import { Component, computed, effect, ElementRef, inject, input, signal, Signal, viewChild } from '@angular/core';
import { EditorSavedState } from '../editor-state.service';
import { EditorSettings } from '../editor-settings.service';
import { Router, RouterLink } from '@angular/router';
import { NgbNav, NgbNavItem, NgbNavLink, NgbNavLinkBase } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { EditorResponsivenessService } from '../editor-responsiveness.service';

@Component({
  selector: 'app-editor-left-pane-header',
  imports: [NgbNav, NgbNavItem, NgbNavLink, RouterLink, FormsModule, NgbNavLinkBase],
  template: `
    <ul ngbNav [activeId]="router.url" class="nav-tabs px-4">
      <li ngbNavItem="/xmp">
        <a ngbNavLink role="button" routerLink="/xmp">
          <i class="bi bi-info-lg pe-1"></i>
          @if (!folded()) {
            <span [class.small]="small()">XMP Metadata</span>
          }
        </a>
      </li>
      <li ngbNavItem="/cii">
        <a ngbNavLink role="button" routerLink="/cii">
          <i class="bi bi-code pe-1"></i>
          @if (!folded()) {
            <span [class.small]="small()">Cross-Industry Invoice</span>
          }
        </a>
      </li>
      <li ngbNavItem="/attachments">
        <a ngbNavLink role="button" routerLink="/attachments">
          <i class="bi bi-paperclip pe-1"></i>
          @if (!folded()) {
            <span [class.small]="small()">Attachments</span>
          }
          ({{ state().attachments.length }})
        </a>
      </li>
      <div class="flex-grow-1"><!--spacer--></div>
      <li ngbNavItem="/settings">
        <a ngbNavLink role="button" routerLink="/settings">
          <i class="bi bi-gear pe-1"></i>
          @if (!folded()) {
            <span [class.small]="small()">Settings</span>
          }
        </a>
      </li>
    </ul>
  `,
})
export class EditorLeftPaneHeaderComponent {
  state = input.required<EditorSavedState>();
  settings = input.required<EditorSettings>();

  protected router = inject(Router);
  protected editorResponsivenessService = inject(EditorResponsivenessService);

  protected small = this.editorResponsivenessService.smallLeftColumn;
  protected folded = this.editorResponsivenessService.foldLeftColumn;
}
