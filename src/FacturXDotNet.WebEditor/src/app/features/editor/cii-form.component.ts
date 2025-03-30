import { Component, computed, effect, input, model, Signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GuidelineSpecifiedDocumentContextParameterId } from '../../core/facturx-models/cii/guideline-specified-document-context-parameter-id';
import { InvoiceTypeCode } from '../../core/facturx-models/cii/invoice-type-code';
import { DateOnlyFormat } from '../../core/facturx-models/cii/date-only-format';
import { CrossIndustryInvoice } from '../../core/facturx-models/cii/cross-industry-invoice';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DateOnly } from '../../core/facturx-models/cii/date-only';
import { VatOnlyTaxSchemeIdentifier } from '../../core/facturx-models/cii/vat-only-tax-scheme-identifier';

@Component({
  selector: 'app-cii-form',
  imports: [ReactiveFormsModule],
  template: `
    <form [formGroup]="form">
      <div formGroupName="exchangedDocumentContext">
        <div class="mb-3">
          <h6>BG-2 - EXCHANGE DOCUMENT CONTEXT</h6>
          @if (showNormal()) {
            <div class="form-text">A group of business terms providing information on the business process and rules applicable to the Invoice document.</div>
          }
        </div>
        <div class="ps-5">
          <div class="mb-3">
            <label class="form-label" for="businessProcessSpecifiedDocumentContextParameterId"> <span class="fw-semibold">BT-23</span> - Business process type </label>
            <input id="businessProcessSpecifiedDocumentContextParameterId" class="form-control" formControlName="businessProcessSpecifiedDocumentContextParameterId" />
            @if (showNormal()) {
              <div id="businessProcessSpecifiedDocumentContextParameterIdHelp" class="form-text">
                Identifies the business process context in which the transaction appears, to enable the Buyer to process the Invoice in an appropriate way.
              </div>
            }
          </div>
          <div class="mb-3">
            <label class="form-label" for="guidelineSpecifiedDocumentContextParameterId"> <span class="fw-semibold">BT-24</span> - Specification identifier </label>
            <select id="guidelineSpecifiedDocumentContextParameterId" class="form-select" formControlName="guidelineSpecifiedDocumentContextParameterId">
              <option value="" class="text-body-tertiary" selected>Choose a profile</option>
              <option value="minimum">Minimum</option>
              <option value="basic-wl">Basic WL</option>
              <option value="basic">Basic</option>
              <option value="en16931">EN 16931</option>
              <option value="extended">Extended</option>
            </select>
            @if (showNormal()) {
              <div id="guidelineSpecifiedDocumentContextParameterIdHelp" class="form-text">
                <p>
                  An identification of the specification containing the total set of rules regarding semantic content, cardinalities and business rules to which the data contained
                  in the instance document conforms.
                </p>
              </div>
              <div id="guidelineSpecifiedDocumentContextParameterIdBusinessRules" class="form-text small">
                <div class="fw-semibold">Business Rules</div>
                <ul>
                  <span class="fw-semibold">BR-1</span
                  >: An Invoice shall have a Specification identifier.
                </ul>
              </div>
            }
            @if (showDetailed()) {
              <div id="guidelineSpecifiedDocumentContextParameterIdDetailedHelp" class="form-text small">
                <i class="bi bi-info-circle"></i>
                This identifies compliance or conformance to the specification. Conformant invoices specify: urn:cen.eu:en16931:2017. Invoices, compliant to a user specification
                may identify that user specification here. No identification scheme is to be used.
              </div>
            }
          </div>
        </div>
      </div>
      <div formGroupName="exchangedDocument">
        <div class="mb-3">
          <h6>BT-1-00 - EXCHANGE DOCUMENT</h6>
        </div>
        <div class="ps-5">
          <div class="mb-3">
            <label class="form-label" for="exchangeDocumentId"> <span class="fw-semibold">BT-1</span> - Invoice number </label>
            <input id="exchangeDocumentId" class="form-control" formControlName="id" />
            @if (showNormal()) {
              <div id="exchangeDocumentIdHelp" class="form-text">A unique identification of the Invoice.</div>
            }
            @if (showDetailed()) {
              <div id="exchangeDocumentIdDetailedHelp" class="form-text">
                <p class="small">
                  <i class="bi bi-info-circle"></i>
                  The sequential number required in Article 226(2) of the directive 2006/112/EC, to uniquely identify the Invoice within the business context, time-frame, operating
                  systems and records of the Seller. It may be based on one or more series of numbers, which may include alphanumeric characters. No identification scheme is to be
                  used.
                </p>
              </div>
            }
          </div>
          <div class="mb-3">
            <label class="form-label" for="typeCode"> <span class="fw-semibold">BT-3</span> - Invoice type code </label>
            <input id="typeCode" class="form-control" formControlName="typeCode" />
            @if (showNormal()) {
              <div id="typeCodeHelp" class="form-text">A code specifying the functional type of the Invoice.</div>
            }
            @if (showDetailed()) {
              <div id="typeCodeDetailedHelp" class="form-text">
                <p class="small">
                  <i class="bi bi-info-circle"></i>
                  Commercial invoices and credit notes are defined according the entries in UNTDID 1001. Other entries of UNTDID 1001 with specific invoices or credit notes may be
                  used if applicable.
                </p>
              </div>
            }
          </div>
        </div>
      </div>
    </form>
  `,
  styles: ``,
})
export class CiiFormComponent {
  value = model.required<CrossIndustryInvoice>();
  verbosity = input<CrossIndustryInvoiceFormVerbosity>('normal');
  disabled = input<boolean>(false);

  protected showMinimal = computed(() => this.verbosity() == 'minimal' || this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showNormal = computed(() => this.verbosity() == 'normal' || this.verbosity() == 'detailed');
  protected showDetailed = computed(() => this.verbosity() == 'detailed');

  constructor() {
    effect(() => {
      if (this.disabled()) {
        this.form.disable({ emitEvent: false });
      } else {
        this.form.enable({ emitEvent: false });
      }
    });

    this.form.valueChanges.pipe(takeUntilDestroyed()).subscribe((v) => {
      this.value.set(this.form.getRawValue());
    });
  }

  protected form = new FormGroup({
    exchangedDocumentContext: new FormGroup({
      businessProcessSpecifiedDocumentContextParameterId: new FormControl('', { nonNullable: true }),
      guidelineSpecifiedDocumentContextParameterId: new FormControl<GuidelineSpecifiedDocumentContextParameterId>('minimum', { nonNullable: true }),
    }),
    exchangedDocument: new FormGroup({
      id: new FormControl('', { nonNullable: true }),
      typeCode: new FormControl<InvoiceTypeCode | undefined>(undefined, { nonNullable: true }),
      issueDateTime: new FormControl<DateOnly | undefined>(undefined, { nonNullable: true }),
      issueDateTimeFormat: new FormControl<DateOnlyFormat>('102-date-only', { nonNullable: true }),
    }),
    supplyChainTradeTransaction: new FormGroup({
      applicableHeaderTradeAgreement: new FormGroup({
        buyerReference: new FormControl('', { nonNullable: true }),
        sellerTradeParty: new FormGroup({
          name: new FormControl('', { nonNullable: true }),
          specifiedLegalOrganization: new FormGroup({
            id: new FormControl<string>('', { nonNullable: true }),
            idSchemeId: new FormControl<string>('', { nonNullable: true }),
          }),
          postalTradeAddress: new FormGroup({
            countryId: new FormControl<string>('', { nonNullable: true }),
          }),
          specifiedTaxRegistration: new FormGroup({
            id: new FormControl<string>('', { nonNullable: true }),
            idSchemeId: new FormControl<VatOnlyTaxSchemeIdentifier>('vat', { nonNullable: true }),
          }),
        }),
        buyerTradeParty: new FormGroup({
          name: new FormControl<string>('', { nonNullable: true }),
          specifiedLegalOrganization: new FormGroup({
            id: new FormControl<string>('', { nonNullable: true }),
            idSchemeId: new FormControl<string>('', { nonNullable: true }),
          }),
        }),
        buyerOrderReferencedDocument: new FormGroup({
          issuerAssignedId: new FormControl<string>('', { nonNullable: true }),
        }),
      }),
      applicableHeaderTradeDelivery: new FormGroup({}),
      applicableHeaderTradeSettlement: new FormGroup({
        invoiceCurrencyCode: new FormControl<string>('', { nonNullable: true }),
        specifiedTradeSettlementHeaderMonetarySummation: new FormGroup({
          taxBasisTotalAmount: new FormControl<number>(0, { nonNullable: true }),
          taxTotalAmount: new FormControl<number>(0, { nonNullable: true }),
          taxTotalAmountCurrencyId: new FormControl<string>('', { nonNullable: true }),
          grandTotalAmount: new FormControl<number>(0, { nonNullable: true }),
          duePayableAmount: new FormControl<number>(0, { nonNullable: true }),
        }),
      }),
    }),
  });
}

export type CrossIndustryInvoiceFormVerbosity = 'minimal' | 'normal' | 'detailed';
