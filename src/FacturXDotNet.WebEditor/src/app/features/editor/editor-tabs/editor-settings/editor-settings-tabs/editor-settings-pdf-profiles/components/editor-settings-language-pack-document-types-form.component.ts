import { Component, inject, input } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IStandardPdfGeneratorLanguagePackDto } from '../../../../../../../core/api/api.models';

@Component({
  selector: 'app-editor-settings-language-pack-document-types-form',
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  imports: [ReactiveFormsModule],
  template: `
    <div [formGroupName]="formGroupName()">
      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-RequestForPayment">71 - Request for payment</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-RequestForPayment"
            formControlName="requestForPayment"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['RequestForPayment'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DebitNoteRelatedToGoodsOrServices">80 - Debit note related to goods or services</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DebitNoteRelatedToGoodsOrServices"
            formControlName="debitNoteRelatedToGoodsOrServices"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DebitNoteRelatedToGoodsOrServices'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CreditNoteRelatedToGoodsOrServices">81 - Credit note related to goods or services</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CreditNoteRelatedToGoodsOrServices"
            formControlName="creditNoteRelatedToGoodsOrServices"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CreditNoteRelatedToGoodsOrServices'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-MeteredServicesInvoice">82 - Metered services invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-MeteredServicesInvoice"
            formControlName="meteredServicesInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['MeteredServicesInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CreditNoteRelatedToFinancialAdjustments">
          83 - Credit note related to financial adjustments
        </label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CreditNoteRelatedToFinancialAdjustments"
            formControlName="cred</divitNoteRelatedToFinancialAdjustments"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CreditNoteRelatedToFinancialAdjustments'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DebitNoteRelatedToFinancialAdjustments">
          84 - Debit note related to financial adjustments
        </label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DebitNoteRelatedToFinancialAdjustments"
            formControlName="debi</divtNoteRelatedToFinancialAdjustments"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DebitNoteRelatedToFinancialAdjustments'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-TaxNotification">102 - Tax notification</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-TaxNotification"
            formControlName="taxNotification"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['TaxNotification'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-InvoicingDataSheet">130 - Invoicing data sheet</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-InvoicingDataSheet"
            formControlName="invoicingDataSheet"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['InvoicingDataSheet'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DirectPaymentValuation">202 - Direct payment valuation</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DirectPaymentValuation"
            formControlName="directPaymentValuation"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DirectPaymentValuation'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ProvisionalPaymentValuation">203 - Provisional payment valuation</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ProvisionalPaymentValuation"
            formControlName="provisionalPaymentValuation"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ProvisionalPaymentValuation'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PaymentValuation">204 - Payment valuation</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PaymentValuation"
            formControlName="paymentValuation"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PaymentValuation'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-InterimApplicationForPayment">211 - Interim application for payment</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-InterimApplicationForPayment"
            formControlName="interimApplicationForPayment"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['InterimApplicationForPayment'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-FinalPaymentRequestBasedOnCompletionOfWork">
          218 - Final payment request based on completion of work
        </label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-FinalPaymentRequestBasedOnCompletionOfWork"
            formControlName="finalPaymentRequestBasedOnCompletionOfWork"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['FinalPaymentRequestBasedOnCompletionOfWork'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PaymentRequestForCompletedUnits">219 - Payment request for completed units</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PaymentRequestForCompletedUnits"
            formControlName="paymentRequestForCompletedUnits"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PaymentRequestForCompletedUnits'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-SelfBilledCreditNote">261 - Self billed credit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-SelfBilledCreditNote"
            formControlName="selfBilledCreditNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['SelfBilledCreditNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ConsolidatedCreditNoteGoodsAndServices">
          262 - Consolidated credit note - goods and services
        </label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ConsolidatedCreditNoteGoodsAndServices"
            formControlName="cons</divolidatedCreditNoteGoodsAndServices"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ConsolidatedCreditNoteGoodsAndServices'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PriceVariationInvoice">295 - Price variation invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PriceVariationInvoice"
            formControlName="priceVariationInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PriceVariationInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CreditNoteForPriceVariation">296 - Credit note for price variation</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CreditNoteForPriceVariation"
            formControlName="creditNoteForPriceVariation"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CreditNoteForPriceVariation'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DelcredereCreditNote">308 - Delcredere credit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DelcredereCreditNote"
            formControlName="delcredereCreditNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DelcredereCreditNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ProformaInvoice">325 - Proforma invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ProformaInvoice"
            formControlName="proformaInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ProformaInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PartialInvoice">326 - Partial invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PartialInvoice"
            formControlName="partialInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PartialInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CommercialInvoiceWhichIncludesPackingList">
          331 - Commercial invoice which includes a packing list
        </label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CommercialInvoiceWhichIncludesPackingList"
            formControlName="commercialInvoiceWhichIncludesPackingList"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CommercialInvoiceWhichIncludesPackingList'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CommercialInvoice">380 - Commercial invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CommercialInvoice"
            formControlName="commercialInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CommercialInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CreditNote">381 - Credit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CreditNote"
            formControlName="creditNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CreditNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CommissionNote">382 - Commission note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CommissionNote"
            formControlName="commissionNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CommissionNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DebitNote">383 - Debit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DebitNote"
            formControlName="debitNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DebitNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CorrectedInvoice">384 - Corrected invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CorrectedInvoice"
            formControlName="correctedInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CorrectedInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ConsolidatedInvoice">385 - Consolidated invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ConsolidatedInvoice"
            formControlName="consolidatedInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ConsolidatedInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PrepaymentInvoice">386 - Prepayment invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PrepaymentInvoice"
            formControlName="prepaymentInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PrepaymentInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-HireInvoice">387 - Hire invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-HireInvoice"
            formControlName="hireInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['HireInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-TaxInvoice">388 - Tax invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-TaxInvoice"
            formControlName="taxInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['TaxInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-SelfBilledInvoice">389 - Self-billed invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-SelfBilledInvoice"
            formControlName="selfBilledInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['SelfBilledInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DelcredereInvoice">390 - Delcredere invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DelcredereInvoice"
            formControlName="delcredereInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DelcredereInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-FactoredInvoice">393 - Factored invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-FactoredInvoice"
            formControlName="factoredInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['FactoredInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-LeaseInvoice">394 - Lease invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-LeaseInvoice"
            formControlName="leaseInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['LeaseInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ConsignmentInvoice">395 - Consignment invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ConsignmentInvoice"
            formControlName="consignmentInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ConsignmentInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-FactoredCreditNote">396 - Factored credit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-FactoredCreditNote"
            formControlName="factoredCreditNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['FactoredCreditNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-OcrPaymentCreditNote">420 - Optical Character Reading (OCR) payment credit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-OcrPaymentCreditNote"
            formControlName="ocrPaymentCreditNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['OcrPaymentCreditNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-DebitAdvice">456 - Debit advice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-DebitAdvice"
            formControlName="debitAdvice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['DebitAdvice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ReversalOfDebit">457 - Reversal of debit</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ReversalOfDebit"
            formControlName="reversalOfDebit"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ReversalOfDebit'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ReversalOfCredit">458 - Reversal of credit</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ReversalOfCredit"
            formControlName="reversalOfCredit"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ReversalOfCredit'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-SelfBilledDebitNote">527 - Self billed debit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-SelfBilledDebitNote"
            formControlName="selfBilledDebitNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['SelfBilledDebitNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ForwardersCreditNote">532 - Forwarder's credit note</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ForwardersCreditNote"
            formControlName="forwardersCreditNote"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ForwardersCreditNote'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ForwardersInvoiceDiscrepancyReport">553 - Forwarder's invoice discrepancy report</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ForwardersInvoiceDiscrepancyReport"
            formControlName="forwardersInvoiceDiscrepancyReport"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ForwardersInvoiceDiscrepancyReport'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-InsurersInvoice">575 - Insurer's invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-InsurersInvoice"
            formControlName="insurersInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['InsurersInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ForwardersInvoice">623 - Forwarder's invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ForwardersInvoice"
            formControlName="forwardersInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ForwardersInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PortChargesDocuments">633 - Port charges documents</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PortChargesDocuments"
            formControlName="portChargesDocuments"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PortChargesDocuments'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-InvoiceInformationForAccountingPurposes">
          751 - Invoice information for accounting purposes
        </label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-InvoiceInformationForAccountingPurposes"
            formControlName="invoiceInformationForAccountingPurposes"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['InvoiceInformationForAccountingPurposes'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-FreightInvoice">780 - Freight invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-FreightInvoice"
            formControlName="freightInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['FreightInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ClaimNotification">817 - Claim notification</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ClaimNotification"
            formControlName="claimNotification"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ClaimNotification'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-ConsularInvoice">870 - Consular invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-ConsularInvoice"
            formControlName="consularInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['ConsularInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PartialConstructionInvoice">875 - Partial construction invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PartialConstructionInvoice"
            formControlName="partialConstructionInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PartialConstructionInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-PartialFinalConstructionInvoice">876 - Partial final construction invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-PartialFinalConstructionInvoice"
            formControlName="partialFinalConstructionInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['PartialFinalConstructionInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-FinalConstructionInvoice">877 - Final construction invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-FinalConstructionInvoice"
            formControlName="finalConstructionInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['FinalConstructionInvoice'] ?? ''"
          />
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-5 col-form-label text-truncate" for="editor-settings-profile-CustomsInvoice">935 - Customs invoice</label>
        <div class="col">
          <input
            class="form-control"
            id="editor-settings-profile-CustomsInvoice"
            formControlName="customsInvoice"
            [placeholder]="baseLanguagePack()?.documentTypeNames?.['CustomsInvoice'] ?? ''"
          />
        </div>
      </div>
    </div>
  `,
  styles: ``,
})
export class EditorSettingsLanguagePackDocumentTypesFormComponent {
  formGroupName = input.required<string>();
  baseLanguagePack = input<IStandardPdfGeneratorLanguagePackDto>();
}
