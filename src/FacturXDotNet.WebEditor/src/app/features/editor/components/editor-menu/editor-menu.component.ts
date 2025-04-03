import { Component, input } from '@angular/core';
import { EditorFileMenuComponent } from './editor-file-menu.component';
import { EditorImportMenuComponent } from './editor-import-menu.component';
import { EditorExportMenuComponent } from './editor-export-menu.component';
import { EditorAboutMenuComponent } from './editor-about-menu.component';

@Component({
  selector: 'app-menu',
  imports: [EditorFileMenuComponent, EditorImportMenuComponent, EditorExportMenuComponent, EditorAboutMenuComponent],
  template: `
    <ul class="nav justify-content-center">
      <app-editor-file-menu></app-editor-file-menu>
      <app-editor-import-menu></app-editor-import-menu>
      <app-editor-export-menu></app-editor-export-menu>
      <app-editor-about-menu></app-editor-about-menu>
    </ul>
  `,
  styles: ``,
})
export class EditorMenuComponent {
  showSelfHostingMenu = input<boolean>(false);
}
