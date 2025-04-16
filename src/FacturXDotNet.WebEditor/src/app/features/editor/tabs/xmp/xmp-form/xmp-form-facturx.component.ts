import { Component, inject } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-xmp-form-facturx',
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
    <div class="card" formGroupName="facturx">
      <div class="card-body">
        <h4 class="card-title sticky-top bg-body">Factur-X</h4>
        <p id="facturx" class="card-text">
          The Factur-X metadata is used to describe the attachment that contains the Cross-Industry Invoice metadata. It is a PDF/A extension that MUST be present for the Factur-X
          document tu be valid.
        </p>

        <div class="row">
          <div class="col">
            <label class="form-label" for="facturxDocumentFileName">File name</label>
            <input id="facturxDocumentFileName" class="editor__control form-control" formControlName="documentFileName" aria-describedby="facturxDocumentFileNameHelp" />
            <p id="facturxDocumentFileNameHelp" class="form-text">The name of the embedded XML document.</p>
          </div>

          <div class="col">
            <label class="form-label" for="facturxDocumentType">File type</label>
            <input id="facturxDocumentType" class="editor__control form-control" formControlName="documentType" aria-describedby="facturxDocumentTypeHelp" />
            <p id="facturxDocumentTypeHelp" class="form-text">The type of the hybrid document in capital letters, e.g. INVOICE or ORDER.</p>
          </div>
        </div>

        <div class="row">
          <div class="col">
            <label class="form-label" for="facturxVersion">Version</label>
            <input id="facturxVersion" class="editor__control form-control" formControlName="version" aria-describedby="facturxVersionHelp" />
            <p id="facturxVersionHelp" class="form-text">The actual version of the standard applying to the embedded XML document.</p>
          </div>

          <div class="col">
            <label class="form-label" for="facturxConformanceLevel">Conformance level</label>
            <select id="facturxConformanceLevel" class="editor__control form-select" formControlName="conformanceLevel" aria-describedby="facturxConformanceLevel">
              <option [ngValue]="undefined" class="text-body-tertiary" selected>No profile</option>
              <option value="Minimum">Minimum</option>
              <option value="Basic-wl">Basic WL</option>
              <option value="Basic">Basic</option>
              <option value="En16931">EN 16931</option>
              <option value="Extended">Extended</option>
            </select>
            <p id="facturxConformanceLevelHelp" class="form-text">The conformance level of the embedded XML document.</p>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class XmpFormFacturxComponent {}
