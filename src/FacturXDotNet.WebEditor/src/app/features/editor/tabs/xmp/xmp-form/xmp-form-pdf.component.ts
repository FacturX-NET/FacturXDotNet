import { Component, inject } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-xmp-form-pdf',
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
    <div class="card" formGroupName="pdf">
      <div class="card-body">
        <h4 id="pdf" class="card-title">PDF</h4>
        <p class="card-text">This namespace specifies properties used with Adobe PDF documents.</p>

        <div class="row">
          <div class="col">
            <label class="form-label" for="pdfPdfVersion">Version</label>
            <input id="pdfPdfVersion" class="editor__control form-control" formControlName="pdfVersion" aria-describedby="pdfPdfVersionHelp" />
            <p id="pdfPdfVersionHelp" class="form-text">The PDF file version (for example: 1.0, 1.3, and so on).</p>
          </div>

          <div class="col">
            <label class="form-label" for="pdfProducer">Producer</label>
            <input id="pdfProducer" class="editor__control form-control" formControlName="producer" aria-describedby="pdfProducerHelp" />
            <p id="pdfProducerHelp" class="form-text">The name of the tool that created the PDF document.</p>
          </div>
        </div>

        <div class="form-check form-switch">
          <input id="pdfTrapped" class="editor__control form-check-input" formControlName="producer" type="checkbox" role="switch" aria-describedby="pdfTrappedHelp" />
          <label class="form-check-label" for="pdfTrapped">Trapped</label>
          <p id="pdfTrappedHelp" class="form-text">True when the document has been trapped.</p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="pdfKeywords">Keywords</label>
          <textarea id="pdfKeywords" class="form-control" formControlName="keywords" aria-describedby="pdfKeywordsHelp"></textarea>
          <p id="pdfKeywordsHelp" class="form-text">Keywords.</p>
        </div>
      </div>
    </div>
  `,
})
export class XmpFormPdfComponent {}
