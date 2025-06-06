import { computed, DestroyRef, inject, Injectable, signal, Signal } from '@angular/core';
import { ImportFileService } from '../../../../core/import-file/import-file.service';
import { ExtractApi } from '../../../../core/api/extract.api';
import { firstValueFrom } from 'rxjs';
import { ICrossIndustryInvoice } from '../../../../core/api/api.models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { downloadBlob, downloadFile } from '../../../../core/utils/download-blob';
import { GenerateApi } from '../../../../core/api/generate.api';
import { CiiFormService } from '../../editor-tabs/editor-cii/cii-form/cii-form.service';
import { EditorStateService } from '../../services/editor-state.service';
import * as pdf from 'pdfjs-dist';
import { EditorSettingsService } from '../../services/editor-settings.service';
import { EditorPdfViewerService } from '../../editor-tabs/editor-pdf-viewer/editor-pdf-viewer.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class EditorMenuService {
  private editorStateService = inject(EditorStateService);
  private router = inject(Router);
  private editorSettingsService = inject(EditorSettingsService);
  private editorPdfViewerService = inject(EditorPdfViewerService);
  private ciiFormService = inject(CiiFormService);
  private importFileService = inject(ImportFileService);
  private extractApi = inject(ExtractApi);
  private generateApi = inject(GenerateApi);
  private destroyRef = inject(DestroyRef);

  public canImport: Signal<boolean> = computed(() => this.editorStateService.savedState.value() !== null);
  public canExport: Signal<boolean> = computed(() => this.editorStateService.savedState.value() !== null);

  public get isImporting() {
    return this.isImportingInternal.asReadonly();
  }

  private isImportingInternal = signal(false);

  public get isExporting() {
    return this.isExportingInternal.asReadonly();
  }

  private isExportingInternal = signal(false);

  async backToWelcomePage(): Promise<void> {
    this.isImportingInternal.set(true);
    try {
      await this.router.navigate(['/', 'welcome']);
    } finally {
      this.isImportingInternal.set(false);
    }
  }

  /**
   * Create a new, blank state.
   */
  async createNewDocument(): Promise<void> {
    await this.editorStateService.new({ name: 'New Invoice' });
  }

  /**
   * Import the PDF data, the Cross-Industry Invoice data and the attachments into a new document.
   */
  async createNewDocumentFromFacturX(): Promise<void> {
    const pdfFile = await this.importFileService.importFile('.pdf');
    if (pdfFile === undefined) {
      return;
    }

    this.isImportingInternal.set(true);
    try {
      const result = await firstValueFrom(this.extractApi.extractXmpAndCrossIndustryInvoice(pdfFile).pipe(takeUntilDestroyed(this.destroyRef)));
      if (result === undefined || result.crossIndustryInvoice === undefined) {
        throw new Error(`Could not extract data from file ${pdfFile.name}.`);
      }

      const attachments = await this.extractPdfAttachments(pdfFile);

      const nameWithoutExtension = pdfFile.name.replace(/\.[^/.]+$/, '');
      await this.editorStateService.new({ name: nameWithoutExtension, xmp: result.xmpMetadata, cii: result.crossIndustryInvoice, pdf: pdfFile, attachments: attachments });
    } finally {
      this.isImportingInternal.set(false);
    }
  }

  /**
   * Import the Cross-Industry Invoice data into a new document.
   */
  async createNewDocumentFromCrossIndustryInvoice(): Promise<void> {
    const xmlFile = await this.importFileService.importFile('.xml');
    if (xmlFile === undefined) {
      return;
    }

    this.isImportingInternal.set(true);
    try {
      const cii = await firstValueFrom(this.extractApi.extractCrossIndustryInvoice(xmlFile).pipe(takeUntilDestroyed(this.destroyRef)));
      if (cii === undefined) {
        throw new Error(`Could not extract data from file ${xmlFile.name}.`);
      }

      const nameWithoutExtension = xmlFile.name.replace(/\.[^/.]+$/, '');
      await this.editorStateService.new({ name: nameWithoutExtension, cii: cii });
    } finally {
      this.isImportingInternal.set(false);
    }
  }

  /**
   * Import the PDF data into a new document.
   */
  async createNewDocumentFromPdf(): Promise<void> {
    const pdfFile = await this.importFileService.importFile('.pdf');
    if (pdfFile === undefined) {
      return;
    }

    this.isImportingInternal.set(true);
    try {
      const xmp = await firstValueFrom(this.extractApi.extractXmpMetadata(pdfFile).pipe(takeUntilDestroyed(this.destroyRef)));
      if (xmp === undefined) {
        throw new Error(`Could not extract data from file ${pdfFile.name}.`);
      }

      const attachments = await this.extractPdfAttachments(pdfFile);

      const nameWithoutExtension = pdfFile.name.replace(/\.[^/.]+$/, '');
      await this.editorStateService.new({ name: nameWithoutExtension, xmp: xmp, pdf: pdfFile, attachments: attachments });
    } finally {
      this.isImportingInternal.set(false);
    }
  }

  /**
   * Imports a Cross-Industry Invoice file and merge its data with the current state.
   */
  async importCrossIndustryInvoiceData(): Promise<void> {
    if (!this.canImport()) {
      throw new Error('Internal Error: no saved state available');
    }

    const file = await this.importFileService.importFile('.xml');
    if (file === undefined) {
      return;
    }

    this.isImportingInternal.set(true);
    try {
      const cii = await firstValueFrom(this.extractApi.extractCrossIndustryInvoice(file).pipe(takeUntilDestroyed(this.destroyRef)));
      if (cii === undefined) {
        throw new Error(`Could not extract data from file ${file.name}.`);
      }

      await this.editorStateService.updateCii(cii);
    } finally {
      this.isImportingInternal.set(false);
    }
  }

  /**
   * Imports a PDF file and merge its data with the current state.
   * The attachments of the PDF are ignored.
   */
  async importPdfImageData(): Promise<void> {
    if (!this.canImport()) {
      throw new Error('Internal Error: no saved state available');
    }

    const pdfFile = await this.importFileService.importFile('.pdf');
    if (pdfFile === undefined) {
      return;
    }

    this.isImportingInternal.set(true);
    try {
      const attachments = await this.extractPdfAttachments(pdfFile);
      await this.editorStateService.updatePdfAndAttachments(pdfFile, attachments);
    } finally {
      this.isImportingInternal.set(false);
    }
  }

  async exportFacturX(): Promise<void> {
    if (!this.canExport()) {
      throw new Error('Internal Error: no saved state available');
    }

    const value = this.editorStateService.savedState.value();
    if (value === null) {
      throw new Error('Internal Error: no saved state available');
    }

    const cii = await this.getValidCii();
    if (cii === undefined) {
      throw new Error('Invalid Cross-Industry Invoice data');
    }

    const pdf = this.getPdf();
    if (pdf === undefined) {
      throw new Error('Internal Error: the PDF is not set');
    }

    this.isExportingInternal.set(true);
    try {
      const facturXFile = await firstValueFrom(this.generateApi.generateFacturX(value.xmp, pdf, cii, ...value.attachments).pipe(takeUntilDestroyed(this.destroyRef)));
      downloadFile(facturXFile, `${value.name}.pdf`);
    } finally {
      this.isExportingInternal.set(false);
    }
  }

  async exportCrossIndustryInvoice(): Promise<void> {
    if (!this.canExport()) {
      throw new Error('Internal Error: no saved state available');
    }

    const value = this.editorStateService.savedState.value();
    if (value === null) {
      throw new Error('Internal Error: no saved state available');
    }

    const cii = await this.getValidCii();
    if (cii === undefined) {
      throw new Error('Invalid Cross-Industry Invoice data');
    }

    this.isExportingInternal.set(true);
    try {
      const ciiFile = await firstValueFrom(this.generateApi.generateCrossIndustryInvoice(cii).pipe(takeUntilDestroyed(this.destroyRef)));
      downloadFile(ciiFile, `${value.name}.xml`);
    } finally {
      this.isExportingInternal.set(false);
    }
  }

  async exportPdfImage(): Promise<void> {
    if (!this.canExport()) {
      throw new Error('Internal Error: no saved state available');
    }

    const value = this.editorStateService.savedState.value();
    if (value === undefined || value === null) {
      throw new Error('Internal Error: no saved state available');
    }

    const pdf = this.getPdf();
    if (pdf === undefined) {
      throw new Error('Internal Error: the PDF is not set');
    }

    this.isExportingInternal.set(true);
    downloadBlob(pdf, value.name ?? 'invoice.pdf');
    this.isExportingInternal.set(false);
  }

  private async getValidCii(): Promise<ICrossIndustryInvoice | undefined> {
    const valid = await this.ciiFormService.validate();
    if (!valid.valid) {
      return undefined;
    }

    return this.ciiFormService.form.getRawValue();
  }

  private getPdf(): Blob | undefined {
    return this.editorPdfViewerService.pdf.value()?.content;
  }

  private async extractPdfAttachments(file: File): Promise<{ name: string; description?: string; content: Uint8Array }[]> {
    const buffer = await file.arrayBuffer();
    const pdfDocumentLoadingTask = pdf.getDocument(buffer);
    const pdfDocument = await pdfDocumentLoadingTask.promise;
    const attachmentsObj = await pdfDocument.getAttachments();

    if (attachmentsObj === null || attachmentsObj === undefined) {
      return [];
    }

    const attachments: { filename: string; description: string; content: Uint8Array }[] = Object.values(attachmentsObj);
    return attachments.filter((a) => a.filename !== 'factur-x.xml').map((a) => ({ name: a.filename, description: a.description, content: a.content }));
  }
}
