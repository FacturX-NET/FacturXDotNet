import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';

@Component({
  selector: 'app-cii-form-exchanged-document',
  imports: [ReactiveFormsModule, CiiFormControlComponent],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  template: `
    <div [formGroupName]="formGroupName()">
      <app-cii-form-control
        term="BT-1"
        name="Invoice number"
        [description]="bt1Description"
        [businessRules]="[{ id: 'BR-2', template: br2 }]"
        [remarks]="[bt1Remark]"
        [chorusProRemarks]="[bt1ChorusProRemark]"
        [settings]="settings()"
        #bt1Control
      >
        <ng-template #bt1Description>A unique identification of the Invoice.</ng-template>
        <ng-template #br2>An Invoice shall have an Invoice number.</ng-template>
        <ng-template #bt1Remark>
          The sequential number required in Article 226(2) of the directive 2006/112/EC, to uniquely identify the Invoice within the business context, time-frame, operating systems
          and records of the Seller. It may be based on one or more series of numbers, which may include alphanumeric characters. No identification scheme is to be used.
        </ng-template>
        <ng-template #bt1ChorusProRemark> The invoice number is limited to 20 characters.</ng-template>

        <input [id]="bt1Control.controlId()" class="form-control" formControlName="id" placeholder="F20250023" />
      </app-cii-form-control>

      <app-cii-form-control
        term="BT-3"
        name="Invoice type code"
        [description]="bt3Description"
        [businessRules]="[{ id: 'BR-4', template: br4 }]"
        [remarks]="[bt3Remark]"
        [chorusProRemarks]="[bt3ChorusProRemark]"
        [settings]="settings()"
        #bt3Control
      >
        <ng-template #bt3Description>A code specifying the functional type of the Invoice.</ng-template>
        <ng-template #br4> An Invoice shall have an Invoice type code.</ng-template>
        <ng-template #bt3Remark>
          Commercial invoices and credit notes are defined according the entries in UNTDID 1001. Other entries of UNTDID 1001 with specific invoices or credit notes may be used if
          applicable.
        </ng-template>
        <ng-template #bt3ChorusProRemark>
          The types of documents used are:
          <ul>
            <li>380: Commercial Invoice</li>
            <li>381: Credit note</li>
            <li>384: Corrected invoice</li>
            <li>389: Self-billed invoice (created by the buyer on behalf of the supplier)</li>
            <li>261: Self billed credit note (not accepted by CHORUSPRO)</li>
            <li>386: Prepayment invoice</li>
            <li>751: Invoice information for accounting purposes (not accepted by CHORUSPRO)</li>
          </ul>
        </ng-template>

        <select [id]="bt3Control.controlId()" class="form-select" formControlName="typeCode">
          <option value="" class="text-body-tertiary" selected>Choose a type</option>
          <option value="RequestForPayment">71 - Request for payment</option>
          <option value="DebitNoteRelatedToGoodsOrServices">80 - Debit note related to goods or services</option>
          <option value="CreditNoteRelatedToGoodsOrServices">81 - Credit note related to goods or services</option>
          <option value="MeteredServicesInvoice">82 - Metered services invoice</option>
          <option value="CreditNoteRelatedToFinancialAdjustments">83 - Credit note related to financial adjustments</option>
          <option value="DebitNoteRelatedToFinancialAdjustments">84 - Debit note related to financial adjustments</option>
          <option value="TaxNotification">102 - Tax notification</option>
          <option value="InvoicingDataSheet">130 - Invoicing data sheet</option>
          <option value="DirectPaymentValuation">202 - Direct payment valuation</option>
          <option value="ProvisionalPaymentValuation">203 - Provisional payment valuation</option>
          <option value="PaymentValuation">204 - Payment valuation</option>
          <option value="InterimApplicationForPayment">211 - Interim application for payment</option>
          <option value="FinalPaymentRequestBasedOnCompletionOfWork">218 - Final payment request based on completion of work</option>
          <option value="PaymentRequestForCompletedUnits">219 - Payment request for completed units</option>
          <option value="SelfBilledCreditNote">261 - Self billed credit note</option>
          <option value="ConsolidatedCreditNoteGoodsAndServices">262 - Consolidated credit note - goods and services</option>
          <option value="PriceVariationInvoice">295 - Price variation invoice</option>
          <option value="CreditNoteForPriceVariation">296 - Credit note for price variation</option>
          <option value="DelcredereCreditNote">308 - Delcredere credit note</option>
          <option value="ProformaInvoice">325 - Proforma invoice</option>
          <option value="PartialInvoice">326 - Partial invoice</option>
          <option value="CommercialInvoiceWhichIncludesPackingList">331 - Commercial invoice which includes a packing list</option>
          <option value="CommercialInvoice">380 - Commercial invoice</option>
          <option value="CreditNote">381 - Credit note</option>
          <option value="CommissionNote">382 - Commission note</option>
          <option value="DebitNote">383 - Debit note</option>
          <option value="CorrectedInvoice">384 - Corrected invoice</option>
          <option value="ConsolidatedInvoice">385 - Consolidated invoice</option>
          <option value="PrepaymentInvoice">386 - Prepayment invoice</option>
          <option value="HireInvoice">387 - Hire invoice</option>
          <option value="TaxInvoice">388 - Tax invoice</option>
          <option value="SelfBilledInvoice">389 - Self-billed invoice</option>
          <option value="DelcredereInvoice">390 - Delcredere invoice</option>
          <option value="FactoredInvoice">393 - Factored invoice</option>
          <option value="LeaseInvoice">394 - Lease invoice</option>
          <option value="ConsignmentInvoice">395 - Consignment invoice</option>
          <option value="FactoredCreditNote">396 - Factored credit note</option>
          <option value="OcrPaymentCreditNote">420 - Optical Character Reading (OCR) payment credit note</option>
          <option value="DebitAdvice">456 - Debit advice</option>
          <option value="ReversalOfDebit">457 - Reversal of debit</option>
          <option value="ReversalOfCredit">458 - Reversal of credit</option>
          <option value="SelfBilledDebitNote">527 - Self billed debit note</option>
          <option value="ForwardersCreditNote">532 - Forwarder's credit note</option>
          <option value="ForwardersInvoiceDiscrepancyReport">553 - Forwarder's invoice discrepancy report</option>
          <option value="InsurersInvoice">575 - Insurer's invoice</option>
          <option value="ForwardersInvoice">623 - Forwarder's invoice</option>
          <option value="PortChargesDocuments">633 - Port charges documents</option>
          <option value="InvoiceInformationForAccountingPurposes">751 - Invoice information for accounting purposes</option>
          <option value="FreightInvoice">780 - Freight invoice</option>
          <option value="ClaimNotification">817 - Claim notification</option>
          <option value="ConsularInvoice">870 - Consular invoice</option>
          <option value="PartialConstructionInvoice">875 - Partial construction invoice</option>
          <option value="PartialFinalConstructionInvoice">876 - Partial final construction invoice</option>
          <option value="FinalConstructionInvoice">877 - Final construction invoice</option>
          <option value="CustomsInvoice">935 - Customs invoice</option>
        </select>
      </app-cii-form-control>

      <div class="d-flex flex-wrap column-gap-4">
        <app-cii-form-control
          term="BT-2"
          name="Invoice issue date"
          [description]="bt2Description"
          [businessRules]="[{ id: 'BR-3', template: br3 }]"
          [chorusProRemarks]="[bt2ChorusProRemark]"
          [settings]="settings()"
          #bt2Control
        >
          <ng-template #bt2Description>The date when the Invoice was issued.</ng-template>
          <ng-template #br3>An Invoice shall have an Invoice issue date.</ng-template>
          <ng-template #bt2ChorusProRemark> The issue date must be before or equal to the deposit date.</ng-template>

          <input [id]="bt2Control.controlId()" class="form-control" formControlName="issueDateTime" type="date" />
        </app-cii-form-control>

        <app-cii-form-control term="BT-2-0" name="Date, format" [description]="bt20Description" [settings]="settings()" #bt20Control>
          <ng-template #bt20Description>Only value "102"</ng-template>

          <select [id]="bt20Control.controlId()" class="form-select" formControlName="issueDateTimeFormat">
            <option value="DateOnly" selected>102 - Date only</option>
          </select>
        </app-cii-form-control>
      </div>
    </div>
  `,
})
export class CiiFormExchangedDocumentComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
