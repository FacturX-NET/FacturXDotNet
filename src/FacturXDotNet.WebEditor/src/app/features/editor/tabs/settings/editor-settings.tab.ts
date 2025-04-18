import { Component, inject } from '@angular/core';
import { EditorResponsivenessService } from '../../editor-responsiveness.service';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { NgbNav, NgbNavItem, NgbNavLink, NgbNavLinkButton } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-settings',
  imports: [RouterOutlet, NgbNav, NgbNavItem, NgbNavLink, RouterLink, NgbNavLinkButton],
  template: `
    <div class="h-100 overflow-auto">
      <div class="container py-4">
        <div class="d-flex">
          <div id="editor__settings-nav" class="flex-shrink-0 ps-xl-3 me-3" [class.small]="small()">
            <div ngbNav [activeId]="router.url" class="nav-pills flex-column" aria-orientation="vertical">
              <ng-container ngbNavItem="/settings">
                <button ngbNavLink class="text-start" role="button" routerLink="/settings"><i class="bi bi-gear"></i> General</button>
              </ng-container>
              <div class="border-top my-3"></div>
              <h6>PDF Generation</h6>
              <ng-container ngbNavItem="/settings/profiles">
                <button ngbNavLink class="text-start" role="button" routerLink="/settings/profiles"><i class="bi bi-file-pdf"></i> PDF Profiles</button>
              </ng-container>
            </div>
          </div>

          <div class="flex-grow-1">
            <router-outlet></router-outlet>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: `
    #editor__settings-nav {
      width: 220px;
    }

    #editor__settings-nav.small {
      width: 180px;
    }
  `,
})
export class EditorSettingsTab {
  protected router = inject(Router);
  private editorResponsivenessService = inject(EditorResponsivenessService);

  protected small = this.editorResponsivenessService.smallLeftColumn;
}
