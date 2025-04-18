import { CanDeactivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { EditorPdfViewerService } from '../components/editor-pdf-viewer/editor-pdf-viewer.service';

export const resetPdfProfileOverride: CanDeactivateFn<unknown> = () => {
  const editorPdfViewerService = inject(EditorPdfViewerService);
  editorPdfViewerService.regenerateAndDisplayStandardPdf();
  return true;
};
