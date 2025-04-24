import { CanDeactivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { EditorPdfViewerService } from '../editor-tabs/editor-pdf-viewer/editor-pdf-viewer.service';

export const resetPdfProfileOverride: CanDeactivateFn<unknown> = () => {
  const editorPdfViewerService = inject(EditorPdfViewerService);
  editorPdfViewerService.regenerateAndDisplayGeneratedPdf();
  return true;
};
