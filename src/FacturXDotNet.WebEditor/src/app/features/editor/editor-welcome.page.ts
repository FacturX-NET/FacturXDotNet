import { Component, inject, Resource } from '@angular/core';
import { EditorMenuService } from './components/editor-menu/editor-menu.service';
import { ToastService } from '../../core/toasts/toast.service';
import { Router, RouterLink } from '@angular/router';
import { EditorSavedState, EditorStateService } from './services/editor-state.service';

@Component({
  selector: 'app-editor-welcome',
  template: `
    <div class="h-100 d-flex flex-column justify-content-center align-items-center py-4 overflow-auto">
      @if (state.value()) {
        <div class="d-flex flex-column align-items-center gap-1">
          <a class="lead text-primary text-center" role="button" routerLink="/">
            Open last document <br />
            {{ state.value()?.name ?? 'last document' }}
          </a>
          <div class="py-1">or</div>
        </div>
      }
      <div class="d-flex flex-column gap-1">
        <a class="lead text-primary" role="button" (click)="createNewDocument()">
          <i class="bi bi-file-excel text-primary"></i>
          Create blank document
        </a>
        <a class="lead text-primary" role="button" (click)="createNewDocumentFromFacturX()">
          <i class="bi bi-file-excel text-primary"></i>
          Open FacturX
        </a>
        <a class="lead text-primary" role="button" (click)="createNewDocumentFromPdf()">
          <i class="bi bi-file-pdf text-primary"></i>
          Open PDF
        </a>
        <a class="lead text-primary" role="button" (click)="createNewDocumentFromCrossIndustryInvoice()">
          <i class="bi bi-file-code text-primary"></i>
          Open Cross-Industry Invoice
        </a>
      </div>
    </div>
  `,
  imports: [RouterLink],
})
export class EditorWelcomePage {
  private editorStateService = inject(EditorStateService);
  private editorMenuService = inject(EditorMenuService);
  private toastService = inject(ToastService);
  private router = inject(Router);

  protected state: Resource<EditorSavedState | null> = this.editorStateService.savedState;

  async createNewDocument() {
    try {
      await this.editorMenuService.createNewDocument();
      await this.router.navigate(['/']);
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not create blank document: ${message}.`);
    }
  }

  async createNewDocumentFromFacturX() {
    try {
      await this.editorMenuService.createNewDocumentFromFacturX();
      await this.router.navigate(['/']);
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not import FacturX file: ${message}.`);
    }
  }

  async createNewDocumentFromCrossIndustryInvoice() {
    try {
      await this.editorMenuService.createNewDocumentFromCrossIndustryInvoice();
      await this.router.navigate(['/']);
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not import Cross-Industry Invoice file: ${message}.`);
    }
  }

  async createNewDocumentFromPdf() {
    try {
      await this.editorMenuService.createNewDocumentFromPdf();
      await this.router.navigate(['/']);
    } catch (error) {
      this.toastService.showError(error, (message) => `Could not import PDF file: ${message}.`);
    }
  }
}
