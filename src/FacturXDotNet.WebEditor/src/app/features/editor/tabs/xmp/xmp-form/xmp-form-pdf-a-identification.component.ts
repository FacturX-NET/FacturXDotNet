import { Component, inject } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-xmp-form-pdf-a-identification',
  imports: [FormsModule, ReactiveFormsModule],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  template: `
    <div class="card" formGroupName="pdfAIdentification">
      <div class="card-body">
        <h4 class="card-title sticky-top bg-body">PDF/A Identification</h4>
        <p id="pdf-a-identification" class="card-text">
          The only mandatory XMP entries are those which indicate that the file is a PDF/A-1 document and its conformance level. The table below lists all properties in the PDF/A
          identification schema. The namespace URI is incorrectly described in ISO 19005-1. Unlike predefined XMP schemas, the namespace prefix is not only preferred, but required.
        </p>

        <div class="row">
          <div class="col-3">
            <label class="form-label" for="pdfAIdentificationPart">Part</label>
            <input id="pdfAIdentificationPart" class="editor__control form-control" formControlName="part" aria-describedby="pdfAIdentificationPartHelp" />
            <p id="pdfAIdentificationPartHelp" class="form-text">PDF/A version identifier.</p>
          </div>
          <div class="col-4">
            <label class="form-label" for="pdfAIdentificationConformance">Conformance</label>
            <select id="pdfAIdentificationConformance" class="editor__control form-select" formControlName="conformance" aria-describedby="pdfAIdentificationConformanceHelp">
              <option value="" class="text-body-tertiary" selected>Choose a conformance level</option>
              <option value="A">A</option>
              <option value="B">B</option>
            </select>
            <p id="pdfAIdentificationConformanceHelp" class="form-text">PDF/A conformance level: A or B.</p>
          </div>
          <div class="col">
            <label class="form-label" for="pdfAIdentificationAmendment">Amendment</label>
            <input id="pdfAIdentificationAmendment" class="editor__control form-control" formControlName="amendment" aria-describedby="pdfAIdentificationAmendmentHelp" />
            <p id="pdfAIdentificationAmendmentHelp" class="form-text">Optional PDF/A amendment identifier.</p>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class XmpFormPdfAIdentificationComponent {}
