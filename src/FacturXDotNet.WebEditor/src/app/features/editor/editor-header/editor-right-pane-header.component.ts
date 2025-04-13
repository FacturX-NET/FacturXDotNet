import { booleanAttribute, Component, computed, input, model } from '@angular/core';
import { NgbNav, NgbNavItem, NgbNavLinkButton } from '@ng-bootstrap/ng-bootstrap';
import { PdfModel } from '../editor-settings.service';

@Component({
  selector: 'app-editor-right-pane-header',
  imports: [NgbNav, NgbNavItem, NgbNavLinkButton],
  template: `
    <div class="d-flex align-items-center">
      <span class="fw-semibold"><i class="bi bi-file-pdf"></i> PDF</span>
      <ul ngbNav [activeId]="actualTab()" (activeIdChange)="tab.set($event)" class="nav-underline small ps-4">
        @if (showImported()) {
          <li ngbNavItem="imported">
            <button ngbNavLink>Imported</button>
          </li>
        }
        <li ngbNavItem="generated">
          <button ngbNavLink>Standard</button>
        </li>
      </ul>
    </div>
  `,
  styles: ``,
})
export class EditorRightPaneHeaderComponent {
  tab = model.required<PdfModel>();
  showImported = input(false, { transform: booleanAttribute });

  protected actualTab = computed(() => {
    const tab = this.tab();
    const showImported = this.showImported();

    if (tab !== 'imported' || showImported) {
      return tab;
    }

    return 'generated';
  });
}
