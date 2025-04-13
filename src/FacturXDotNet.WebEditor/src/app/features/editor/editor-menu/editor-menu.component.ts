import { Component, inject, input, output, viewChild } from '@angular/core';
import { EditorFileMenuComponent } from './editor-file-menu.component';
import { EditorImportMenuComponent } from './editor-import-menu.component';
import { EditorExportMenuComponent } from './editor-export-menu.component';
import { EditorAboutMenuComponent } from './editor-about-menu.component';
import { EditorMenuService } from './editor-menu.service';

@Component({
  selector: 'app-menu',
  imports: [EditorFileMenuComponent, EditorImportMenuComponent, EditorExportMenuComponent, EditorAboutMenuComponent],
  template: `
    <ul class="nav align-items-center justify-content-center">
      <app-editor-file-menu #fileMenu></app-editor-file-menu>

      @if (canImport()) {
        <app-editor-import-menu #importMenu></app-editor-import-menu>
      }

      @if (canExport()) {
        <app-editor-export-menu #exportMenu (exporting)="exporting.emit($event)"></app-editor-export-menu>
      }

      <app-editor-about-menu [showSelfHostingMenu]="showSelfHostingMenu()" #aboutMenu></app-editor-about-menu>
    </ul>
  `,
})
export class EditorMenuComponent {
  showSelfHostingMenu = input<boolean>(false);
  exporting = output<boolean>();

  private editorMenuService = inject(EditorMenuService);
  protected canImport = this.editorMenuService.canImport;
  protected canExport = this.editorMenuService.canExport;
}
