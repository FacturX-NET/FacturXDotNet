import { Component, input, output, viewChild } from '@angular/core';
import { EditorFileMenuComponent } from './editor-file-menu.component';
import { EditorImportMenuComponent } from './editor-import-menu.component';
import { EditorExportMenuComponent } from './editor-export-menu.component';
import { EditorAboutMenuComponent } from './editor-about-menu.component';

@Component({
  selector: 'app-menu',
  imports: [EditorFileMenuComponent, EditorImportMenuComponent, EditorExportMenuComponent, EditorAboutMenuComponent],
  template: `
    <ul class="nav justify-content-center">
      <app-editor-file-menu #fileMenu></app-editor-file-menu>
      <app-editor-import-menu #importMenu></app-editor-import-menu>
      <app-editor-export-menu #exportMenu (exporting)="exporting.emit($event)"></app-editor-export-menu>
      <app-editor-about-menu #aboutMenu></app-editor-about-menu>
    </ul>
  `,
})
export class EditorMenuComponent {
  showSelfHostingMenu = input<boolean>(false);
  exporting = output<boolean>();

  fileMenu = viewChild<EditorFileMenuComponent>('fileMenu');
  importMenu = viewChild<EditorImportMenuComponent>('importMenu');
  exportMenu = viewChild<EditorExportMenuComponent>('exportMenu');
  aboutMenu = viewChild<EditorAboutMenuComponent>('aboutMenu');
}
