import { Component, inject, input } from '@angular/core';
import { ICrossIndustryInvoice } from '../../../../core/api/api.models';
import { CiiSummaryNodeComponent } from './cii-summary-node.component';
import { EditorSettings } from '../../editor-settings.service';
import { AbstractControl } from '@angular/forms';
import { CiiFormService } from '../cii-form/cii-form.service';

@Component({
  selector: 'app-cii-summary',
  imports: [CiiSummaryNodeComponent],
  template: `
    <div id="editor__cii-summary-content">
      @for (node of menus; track node.term) {
        <app-cii-summary-node [node]="node" [settings]="settings()" />
      }
    </div>
  `,
})
export class CiiSummaryComponent {
  value = input.required<ICrossIndustryInvoice>();
  settings = input.required<EditorSettings>();

  private ciiFormService = inject(CiiFormService);

  protected menus: CiiSummaryNode[] = [
    {
      term: 'BG-2',
      name: 'EXCHANGE DOCUMENT CONTEXT',
      disabled: true,
      control: this.ciiFormService.form.controls.exchangedDocumentContext,
      children: [
        {
          term: 'BT-23',
          name: 'Business process type',
          control: this.ciiFormService.form.controls.exchangedDocumentContext.controls.businessProcessSpecifiedDocumentContextParameterId,
        },
        {
          term: 'BT-24',
          name: 'Specification identifier',
          control: this.ciiFormService.form.controls.exchangedDocumentContext.controls.guidelineSpecifiedDocumentContextParameterId,
          businessRules: ['BR-1'],
          hasRemarks: true,
        },
      ],
    },
    {
      term: 'BT-1-00',
      name: 'EXCHANGED DOCUMENT',
      disabled: true,
      control: this.ciiFormService.form.controls.exchangedDocument,
      children: [
        {
          term: 'BT-1',
          name: 'Invoice number',
          control: this.ciiFormService.form.controls.exchangedDocument.controls.id,
          businessRules: ['BR-2'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          term: 'BT-3',
          name: 'Invoice type code',
          control: this.ciiFormService.form.controls.exchangedDocument.controls.typeCode,
          businessRules: ['BR-4'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          term: 'BT-2',
          name: 'Invoice issue date',
          control: this.ciiFormService.form.controls.exchangedDocument.controls.issueDateTime,
          businessRules: ['BR-3'],
          hasChorusProRemarks: true,
          children: [
            {
              term: 'BT-2-0',
              name: 'Date, format',
              control: this.ciiFormService.form.controls.exchangedDocument.controls.issueDateTimeFormat,
            },
          ],
        },
      ],
    },
    {
      term: 'BG-25-00',
      name: 'SUPPLY CHAIN TRADE TRANSACTION',
      control: this.ciiFormService.form.controls.supplyChainTradeTransaction,
      disabled: true,
      children: [
        {
          term: 'BT-10-00',
          name: 'HEADER TRADE AGREEMENT',
          control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement,
          disabled: true,
          children: [
            {
              term: 'BT-10',
              name: 'Buyer reference',
              control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerReference,
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              term: 'BG-4',
              name: 'SELLER',
              control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty,
              disabled: true,
              children: [
                {
                  term: 'BT-27',
                  name: 'Seller name',
                  control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.name,
                  businessRules: ['BR-6'],
                },
                {
                  term: 'BT-30-00',
                  name: 'SELLER LEGAL ORGANIZATION',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls
                      .specifiedLegalOrganization,
                  disabled: true,
                  children: [
                    {
                      term: 'BT-30',
                      name: 'Seller legal registration identifier',
                      control:
                        this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls
                          .specifiedLegalOrganization.controls.id,
                      businessRules: ['BR-CO-26'],
                      children: [
                        {
                          term: 'BT-30-1',
                          name: 'Scheme identifier',
                          control:
                            this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls
                              .specifiedLegalOrganization.controls.idSchemeId,
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
                {
                  term: 'BG-5',
                  name: 'SELLER POSTAL ADDRESS',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress,
                  disabled: true,
                  businessRules: ['BR-8'],
                  hasRemarks: true,
                  children: [
                    {
                      term: 'BT-40',
                      name: 'Seller country code',
                      control:
                        this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls.postalTradeAddress
                          .controls.countryId,
                      businessRules: ['BR-9'],
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  term: 'BT-31-00',
                  name: 'SELLER VAT IDENTIFIER',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls
                      .specifiedTaxRegistration,
                  disabled: true,
                  children: [
                    {
                      term: 'BT-31',
                      name: 'Seller VAT identifier',
                      control:
                        this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls
                          .specifiedTaxRegistration.controls.id,
                      businessRules: ['BR-CO-9', 'BR-CO-26'],
                      hasRemarks: true,
                      children: [
                        {
                          term: 'BT-31-0',
                          name: 'Tax Scheme identifier',
                          control:
                            this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.sellerTradeParty.controls
                              .specifiedTaxRegistration.controls.idSchemeId,
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
              ],
            },
            {
              term: 'BG-7',
              name: 'BUYER',
              control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty,
              disabled: true,
              children: [
                {
                  term: 'BT-44',
                  name: 'Buyer name',
                  control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls.name,
                  businessRules: ['BR-7'],
                },
                {
                  term: 'BT-47-00',
                  name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls
                      .specifiedLegalOrganization,
                  disabled: true,
                  children: [
                    {
                      term: 'BT-47',
                      name: 'Buyer legal registration identifier',
                      control:
                        this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls
                          .specifiedLegalOrganization.controls.id,
                      hasRemarks: true,
                      hasChorusProRemarks: true,
                      children: [
                        {
                          term: 'BT-47-1',
                          name: 'Scheme identifier',
                          control:
                            this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerTradeParty.controls
                              .specifiedLegalOrganization.controls.idSchemeId,
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
              ],
            },
            {
              term: 'BT-13-00',
              name: 'PURCHASE ORDER REFERENCE',
              control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument,
              disabled: true,
              children: [
                {
                  term: 'BT-13',
                  name: 'Purchase order reference',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeAgreement.controls.buyerOrderReferencedDocument.controls
                      .issuerAssignedId,
                  hasChorusProRemarks: true,
                },
              ],
            },
          ],
        },
        {
          term: 'BG-13-00',
          name: 'DELIVERY INFORMATION',
          control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeDelivery,
          disabled: true,
        },
        {
          term: 'BG-19',
          name: 'HEADER TRADE SETTLEMENT DIRECT DEBIT',
          control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement,
          disabled: true,
          hasRemarks: true,
          hasChorusProRemarks: true,
          children: [
            {
              term: 'BT-5',
              name: 'Invoice currency code',
              control: this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.invoiceCurrencyCode,
              businessRules: ['BR-5'],
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              term: 'BG-22',
              name: 'DOCUMENT TOTALS',
              control:
                this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation,
              disabled: true,
              hasRemarks: true,
              hasChorusProRemarks: true,
              children: [
                {
                  term: 'BT-109',
                  name: 'Total amount without VAT',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation
                      .controls.taxBasisTotalAmount,
                  businessRules: ['BR-13', 'BR-CO-13'],
                  hasRemarks: true,
                },
                {
                  term: 'BT-110',
                  name: 'Total VAT amount',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation
                      .controls.taxTotalAmount,
                  businessRules: ['BR-CO-14'],
                  hasRemarks: true,
                  children: [
                    {
                      term: 'BT-110-1',
                      name: 'VAT currency',
                      control:
                        this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls
                          .specifiedTradeSettlementHeaderMonetarySummation.controls.taxTotalAmountCurrencyId,
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  term: 'BT-112',
                  name: 'Total amount with VAT',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation
                      .controls.grandTotalAmount,
                  businessRules: ['BR-14', 'BR-CO-15', 'BR-FXEXT-CO-15'],
                  hasRemarks: true,
                },
                {
                  term: 'BT-115',
                  name: 'Amount due for payment',
                  control:
                    this.ciiFormService.form.controls.supplyChainTradeTransaction.controls.applicableHeaderTradeSettlement.controls.specifiedTradeSettlementHeaderMonetarySummation
                      .controls.duePayableAmount,
                  businessRules: ['BR-15', 'BR-CO-16'],
                  hasRemarks: true,
                },
              ],
            },
          ],
        },
      ],
    },
  ];
}

export interface CiiSummaryNode {
  term: string;
  name: string;
  control: AbstractControl;
  disabled?: boolean;
  businessRules?: string[];
  hasRemarks?: boolean;
  hasChorusProRemarks?: boolean;
  children?: CiiSummaryNode[];
}
