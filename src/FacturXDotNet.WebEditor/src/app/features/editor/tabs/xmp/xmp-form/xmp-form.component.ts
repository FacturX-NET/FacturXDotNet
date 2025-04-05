import { Component, inject, input, model } from '@angular/core';
import { ICrossIndustryInvoice, IXmpMetadata } from '../../../../../core/api/api.models';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormService } from '../../cii/cii-form/cii-form.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { XmpFormService } from './xmp-form.service';
import { XmpFormFacturxComponent } from './xmp-form-facturx.component';
import { XmpFormPdfAIdentificationComponent } from './xmp-form-pdf-a-identification.component';
import { XmpFormBasicComponent } from './xmp-form-basic.component';
import { XmpFormPdfComponent } from './xmp-form-pdf.component';
import { XmpFormDublinCoreComponent } from './xmp-form-dublin-core.component';

@Component({
  selector: 'app-xmp-form',
  imports: [
    ReactiveFormsModule,
    XmpFormBasicComponent,
    XmpFormPdfAIdentificationComponent,
    XmpFormBasicComponent,
    XmpFormFacturxComponent,
    XmpFormPdfComponent,
    XmpFormDublinCoreComponent,
  ],
  template: `
    <form [formGroup]="form">
      <div class="d-flex flex-column gap-4">
        <app-xmp-form-pdf-a-identification />
        <app-xmp-form-basic />
        <app-xmp-form-pdf />
        <app-xmp-form-dublin-core />
        <app-xmp-form-facturx />
      </div>
    </form>
  `,
})
export class XmpFormComponent {
  value = model.required<IXmpMetadata>();
  settings = input<EditorSettings>();

  private xmpFormService = inject(XmpFormService);

  protected get form(): FormGroup {
    return this.xmpFormService.form;
  }
}
