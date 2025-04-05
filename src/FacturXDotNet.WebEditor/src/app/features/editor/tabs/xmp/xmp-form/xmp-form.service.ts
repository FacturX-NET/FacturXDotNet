import { effect, inject, Injectable } from '@angular/core';
import { debounceTime, from, Subject, switchMap } from 'rxjs';
import { EditorSavedState, EditorStateService } from '../../../editor-state.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CiiFormService } from '../../cii/cii-form/cii-form.service';
import { FormControl, FormGroup } from '@angular/forms';
import { IXmpMetadata, XmpFacturXConformanceLevel, XmpFacturXDocumentType, XmpPdfAConformanceLevel } from '../../../../../core/api/api.models';

@Injectable({
  providedIn: 'root',
})
export class XmpFormService {
  private saveSubject = new Subject<EditorSavedState>();

  private editorStateService = inject(EditorStateService);

  constructor() {
    const ciiFormService = inject(CiiFormService);

    this.saveSubject
      .pipe(
        debounceTime(1000),
        switchMap((state) => from(this.editorStateService.updateXmp(state.xmp))),
        takeUntilDestroyed(),
      )
      .subscribe();

    effect(() => {
      const value = this.editorStateService.savedState.value()?.xmp;
      if (value === undefined) {
        return;
      }

      this.form.reset(this.toFormValue(value), { emitEvent: false });
    });

    this.form.valueChanges.pipe(takeUntilDestroyed()).subscribe((event) => {
      const value = this.editorStateService.savedState.value();
      if (value === null) {
        return;
      }

      this.saveSubject.next({ ...value, xmp: this.toXmpMetadata(this.form.getRawValue()) });
    });

    const ciiType = ciiFormService.form.controls.exchangedDocumentContext.controls.guidelineSpecifiedDocumentContextParameterId.value;
    this.form.controls.facturx.controls.conformanceLevel.setValue(ciiType, { emitEvent: false });

    effect(() => {
      const value = this.editorStateService.savedState.value();
      if (value === null) {
        return;
      }

      const ciiType = value.cii.exchangedDocumentContext?.guidelineSpecifiedDocumentContextParameterId;
      if (ciiType === null) {
        return;
      }

      this.form.controls.facturx.controls.conformanceLevel.setValue(ciiType, { emitEvent: false });
    });

    this.form.controls.pdfAIdentification.controls.amendment.disable({ emitEvent: false });
    this.form.controls.pdfAIdentification.controls.conformance.disable({ emitEvent: false });
    this.form.controls.pdfAIdentification.controls.part.disable({ emitEvent: false });

    this.form.controls.basic.controls.creatorTool.disable({ emitEvent: false });

    this.form.controls.facturx.controls.documentFileName.disable({ emitEvent: false });
    this.form.controls.facturx.controls.documentType.disable({ emitEvent: false });
    this.form.controls.facturx.controls.version.disable({ emitEvent: false });
    this.form.controls.facturx.controls.conformanceLevel.disable({ emitEvent: false });
  }

  form = new FormGroup({
    pdfAIdentification: new FormGroup({
      amendment: new FormControl('', { nonNullable: true }),
      conformance: new FormControl<XmpPdfAConformanceLevel>('B', { nonNullable: true }),
      part: new FormControl<number>(3, { nonNullable: true }),
    }),
    basic: new FormGroup({
      identifier: new FormControl('', { nonNullable: true }),
      createDate: new FormControl('', { nonNullable: true }),
      creatorTool: new FormControl('FacturX.NET', { nonNullable: true }),
      label: new FormControl('', { nonNullable: true }),
      metadataDate: new FormControl('', { nonNullable: true }),
      modifyDate: new FormControl('', { nonNullable: true }),
      rating: new FormControl<number>(0, { nonNullable: true }),
      baseUrl: new FormControl('', { nonNullable: true }),
      nickname: new FormControl('', { nonNullable: true }),
    }),
    pdf: new FormGroup({
      keywords: new FormControl('', { nonNullable: true }),
      pdfVersion: new FormControl('', { nonNullable: true }),
      producer: new FormControl('', { nonNullable: true }),
      trapped: new FormControl<boolean>(false, { nonNullable: true }),
    }),
    dublinCore: new FormGroup({
      contributor: new FormControl('', { nonNullable: true }),
      coverage: new FormControl('', { nonNullable: true }),
      creator: new FormControl('', { nonNullable: true }),
      date: new FormControl('', { nonNullable: true }),
      description: new FormControl('', { nonNullable: true }),
      format: new FormControl('', { nonNullable: true }),
      identifier: new FormControl('', { nonNullable: true }),
      language: new FormControl('', { nonNullable: true }),
      publisher: new FormControl('', { nonNullable: true }),
      relation: new FormControl('', { nonNullable: true }),
      rights: new FormControl('', { nonNullable: true }),
      source: new FormControl('', { nonNullable: true }),
      subject: new FormControl('', { nonNullable: true }),
      title: new FormControl('', { nonNullable: true }),
      type: new FormControl('', { nonNullable: true }),
    }),
    facturx: new FormGroup({
      documentFileName: new FormControl('factur-x.xml', { nonNullable: true }),
      documentType: new FormControl<XmpFacturXDocumentType>('Invoice', { nonNullable: true }),
      version: new FormControl('1.0', { nonNullable: true }),
      conformanceLevel: new FormControl<XmpFacturXConformanceLevel | undefined>(undefined, { nonNullable: true }),
    }),
  });

  private toXmpMetadata(value: typeof this.form.value): IXmpMetadata {
    return {
      pdfAIdentification: {
        amendment: value.pdfAIdentification?.amendment,
        conformance: value.pdfAIdentification?.conformance,
        part: value.pdfAIdentification?.part,
      },
      basic: {
        identifier: value.basic?.identifier === undefined ? undefined : [value.basic?.identifier],
        createDate: value.basic?.createDate,
        creatorTool: value.basic?.creatorTool,
        label: value.basic?.label,
        metadataDate: value.basic?.metadataDate,
        modifyDate: value.basic?.modifyDate,
        rating: value.basic?.rating,
        baseUrl: value.basic?.baseUrl,
        nickname: value.basic?.nickname,
      },
      pdf: {
        keywords: value.pdf?.keywords,
        pdfVersion: value.pdf?.pdfVersion,
        producer: value.pdf?.producer,
        trapped: value.pdf?.trapped,
      },
      dublinCore: {
        contributor: value.dublinCore?.contributor === undefined ? undefined : [value.dublinCore.contributor],
        coverage: value.dublinCore?.coverage,
        creator: value.dublinCore?.creator === undefined ? undefined : [value.dublinCore.creator],
        date: value.dublinCore?.date === undefined ? undefined : [value.dublinCore.date],
        description: value.dublinCore?.description === undefined ? undefined : [value.dublinCore.description],
        format: value.dublinCore?.format,
        identifier: value.dublinCore?.identifier,
        language: value.dublinCore?.language === undefined ? undefined : [value.dublinCore.language],
        publisher: value.dublinCore?.publisher === undefined ? undefined : [value.dublinCore.publisher],
        relation: value.dublinCore?.relation === undefined ? undefined : [value.dublinCore.relation],
        rights: value.dublinCore?.rights === undefined ? undefined : [value.dublinCore.rights],
        source: value.dublinCore?.source,
        subject: value.dublinCore?.subject === undefined ? undefined : [value.dublinCore.subject],
        title: value.dublinCore?.title === undefined ? undefined : [value.dublinCore.title],
        type: value.dublinCore?.type === undefined ? undefined : [value.dublinCore.type],
      },
      facturX: {
        documentFileName: value.facturx?.documentFileName,
        documentType: value.facturx?.documentType,
        version: value.facturx?.version,
        conformanceLevel: value.facturx?.conformanceLevel,
      },
    };
  }

  private toFormValue(value: IXmpMetadata): typeof this.form.value {
    return {
      pdfAIdentification: {
        amendment: value.pdfAIdentification?.amendment,
        conformance: value.pdfAIdentification?.conformance,
        part: value.pdfAIdentification?.part,
      },
      basic: {
        identifier: value.basic?.identifier?.[0],
        createDate: value.basic?.createDate,
        creatorTool: value.basic?.creatorTool,
        label: value.basic?.label,
        metadataDate: value.basic?.metadataDate,
        modifyDate: value.basic?.modifyDate,
        rating: value.basic?.rating,
        baseUrl: value.basic?.baseUrl,
        nickname: value.basic?.nickname,
      },
      pdf: {
        keywords: value.pdf?.keywords,
        pdfVersion: value.pdf?.pdfVersion,
        producer: value.pdf?.producer,
        trapped: value.pdf?.trapped,
      },
      dublinCore: {
        contributor: value.dublinCore?.contributor?.[0],
        coverage: value.dublinCore?.coverage,
        creator: value.dublinCore?.creator?.[0],
        date: value.dublinCore?.date?.[0],
        description: value.dublinCore?.description?.[0],
        format: value.dublinCore?.format,
        identifier: value.dublinCore?.identifier,
        language: value.dublinCore?.language?.[0],
        publisher: value.dublinCore?.publisher?.[0],
        relation: value.dublinCore?.relation?.[0],
        rights: value.dublinCore?.rights?.[0],
        source: value.dublinCore?.source,
        subject: value.dublinCore?.subject?.[0],
        title: value.dublinCore?.title?.[0],
        type: value.dublinCore?.type?.[0],
      },
      facturx: {
        documentFileName: value.facturX?.documentFileName,
        documentType: value.facturX?.documentType,
        version: value.facturX?.version,
        conformanceLevel: value.facturX?.conformanceLevel,
      },
    };
  }
}
