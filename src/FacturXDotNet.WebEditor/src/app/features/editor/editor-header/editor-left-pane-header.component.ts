import { Component, computed, effect, ElementRef, inject, input, signal, Signal, viewChild } from '@angular/core';
import { EditorSavedState } from '../editor-state.service';
import { EditorSettings } from '../editor-settings.service';
import { Router, RouterLink } from '@angular/router';
import { NgbNav, NgbNavItem, NgbNavLink, NgbNavLinkBase } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-editor-left-pane-header',
  imports: [NgbNav, NgbNavItem, NgbNavLink, RouterLink, FormsModule, NgbNavLinkBase],
  template: `
    <ul ngbNav [activeId]="router.url" class="nav-tabs px-4">
      <li ngbNavItem="/xmp">
        <a ngbNavLink role="button" routerLink="/xmp">
          <i class="bi bi-info-lg"></i>
          @if (!folded()) {
            XMP Metadata
          }
        </a>
      </li>
      <li ngbNavItem="/cii">
        <a ngbNavLink role="button" routerLink="/cii">
          <i class="bi bi-code"></i>
          @if (!folded()) {
            Cross-Industry Invoice
          }
        </a>
      </li>
      <li ngbNavItem="/attachments">
        <a ngbNavLink role="button" routerLink="/attachments">
          <i class="bi bi-paperclip"></i>
          @if (!folded()) {
            Attachments
          }
          ({{ state().attachments.length }})
        </a>
      </li>
      <div class="flex-grow-1"><!--spacer--></div>
      <li ngbNavItem="/settings">
        <a ngbNavLink role="button" routerLink="/settings">
          <i class="bi bi-gear"></i>
          @if (!folded()) {
            Settings
          }
        </a>
      </li>
    </ul>
  `,
})
export class EditorLeftPaneHeaderComponent {
  state = input.required<EditorSavedState>();
  width = input.required<number>();
  settings = input.required<EditorSettings>();

  protected router = inject(Router);
  protected folded = computed(() => {
    const width = this.width();
    return width !== undefined && width < 800;
  });

  protected async changeTab(route: string) {
    await this.router.navigateByUrl(route);
  }
}
