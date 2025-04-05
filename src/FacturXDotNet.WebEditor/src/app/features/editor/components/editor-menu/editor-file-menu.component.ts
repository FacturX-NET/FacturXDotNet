import { Component, DestroyRef, inject } from '@angular/core';
import { EditorMenuService } from './editor-menu.service';
import { ToastService } from '../../../../core/toasts/toast.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toastError } from '../../../../core/utils/toast-error';

@Component({
  selector: 'app-editor-file-menu',
  template: `
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle px-4 text-light" role="button" data-bs-toggle="dropdown" aria-expanded="false">File</a>
      <ul class="dropdown-menu">
        <li>
          <a class="dropdown-item" (click)="createNewFacturXDocument()">Create blank FacturX document</a>
          <a class="dropdown-item" (click)="importFacturX()">Open FacturX document</a>
        </li>
      </ul>
    </li>
  `,
})
export class EditorFileMenuComponent {
  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private destroyRef = inject(DestroyRef);

  protected importFacturX() {
    this.editorMenuService.importFacturX().pipe(toastError(this.toastService, 'Could not open FacturX document.'), takeUntilDestroyed(this.destroyRef)).subscribe();
  }

  protected createNewFacturXDocument() {
    this.editorMenuService
      .createNewFacturXDocument()
      .pipe(toastError(this.toastService, 'Could not create new FacturX document.'), takeUntilDestroyed(this.destroyRef))
      .subscribe();
  }
}
