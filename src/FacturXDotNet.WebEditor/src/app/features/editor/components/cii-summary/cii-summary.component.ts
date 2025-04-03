import { Component, input } from '@angular/core';
import { CrossIndustryInvoice } from '../../../../core/facturx-models/cii/cross-industry-invoice';
import { CiiSummaryNodeComponent } from './cii-summary-node.component';
import { EditorSettings } from '../../editor-settings.service';

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
  value = input.required<CrossIndustryInvoice>();
  settings = input.required<EditorSettings>();

  protected menus: CiiSummaryNode[] = [
    {
      term: 'BG-2',
      name: 'EXCHANGE DOCUMENT CONTEXT',
      disabled: true,
      children: [
        {
          term: 'BT-23',
          name: 'Business process type',
        },
        {
          term: 'BT-24',
          name: 'Specification identifier',
          businessRules: ['BR-1'],
          hasRemarks: true,
        },
      ],
    },
    {
      term: 'BT-1-00',
      name: 'EXCHANGED DOCUMENT',
      disabled: true,
      children: [
        {
          term: 'BT-1',
          name: 'Invoice number',
          businessRules: ['BR-2'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          term: 'BT-3',
          name: 'Invoice type code',
          businessRules: ['BR-4'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          term: 'BT-2',
          name: 'Invoice issue date',
          businessRules: ['BR-3'],
          hasChorusProRemarks: true,
          children: [
            {
              term: 'BT-2-0',
              name: 'Date, format',
            },
          ],
        },
      ],
    },
    {
      term: 'BG-25-00',
      name: 'SUPPLY CHAIN TRADE TRANSACTION',
      disabled: true,
      children: [
        {
          term: 'BT-10-00',
          name: 'HEADER TRADE AGREEMENT',
          disabled: true,
          children: [
            {
              term: 'BT-10',
              name: 'Buyer reference',
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              term: 'BG-4',
              name: 'SELLER',
              disabled: true,
              children: [
                {
                  term: 'BT-27',
                  name: 'Seller name',
                  businessRules: ['BR-6'],
                },
                {
                  term: 'BT-30-00',
                  name: 'SELLER LEGAL ORGANIZATION',
                  disabled: true,
                  children: [
                    {
                      term: 'BT-30',
                      name: 'Seller legal registration identifier',
                      businessRules: ['BR-CO-26'],
                      children: [
                        {
                          term: 'BT-30-1',
                          name: 'Scheme identifier',
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
                {
                  term: 'BG-5',
                  name: 'SELLER POSTAL ADDRESS',
                  disabled: true,
                  businessRules: ['BR-8'],
                  hasRemarks: true,
                  children: [
                    {
                      term: 'BT-40',
                      name: 'Seller country code',
                      businessRules: ['BR-9'],
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  term: 'BT-31-00',
                  name: 'SELLER VAT IDENTIFIER',
                  disabled: true,
                  children: [
                    {
                      term: 'BT-31',
                      name: 'Seller VAT identifier',
                      businessRules: ['BR-CO-9', 'BR-CO-26'],
                      hasRemarks: true,
                      children: [
                        {
                          term: 'BT-31-0',
                          name: 'Tax Scheme identifier',
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
              disabled: true,
              children: [
                {
                  term: 'BT-44',
                  name: 'Buyer name',
                  businessRules: ['BR-7'],
                },
                {
                  term: 'BT-47-00',
                  name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
                  disabled: true,
                  children: [
                    {
                      term: 'BT-47',
                      name: 'Buyer legal registration identifier',
                      hasRemarks: true,
                      hasChorusProRemarks: true,
                      children: [
                        {
                          term: 'BT-47-1',
                          name: 'Scheme identifier',
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
              disabled: true,
              children: [
                {
                  term: 'BT-13',
                  name: 'Purchase order reference',
                  hasChorusProRemarks: true,
                },
              ],
            },
          ],
        },
        {
          term: 'BG-13-00',
          name: 'DELIVERY INFORMATION',
          disabled: true,
        },
        {
          term: 'BG-19',
          name: 'HEADER TRADE SETTLEMENT DIRECT DEBIT',
          disabled: true,
          hasRemarks: true,
          hasChorusProRemarks: true,
          children: [
            {
              term: 'BT-5',
              name: 'Invoice currency code',
              businessRules: ['BR-5'],
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              term: 'BG-22',
              name: 'DOCUMENT TOTALS',
              disabled: true,
              hasRemarks: true,
              hasChorusProRemarks: true,
              children: [
                {
                  term: 'BT-109',
                  name: 'Total amount without VAT',
                  businessRules: ['BR-13', 'BR-CO-13'],
                  hasRemarks: true,
                },
                {
                  term: 'BT-110',
                  name: 'Total VAT amount',
                  businessRules: ['BR-CO-14'],
                  hasRemarks: true,
                  children: [
                    {
                      term: 'BT-110-1',
                      name: 'VAT currency',
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  term: 'BT-112',
                  name: 'Total amount with VAT',
                  businessRules: ['BR-14', 'BR-CO-15', 'BR-FXEXT-CO-15'],
                  hasRemarks: true,
                },
                {
                  term: 'BT-115',
                  name: 'Amount due for payment',
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
  disabled?: boolean;
  businessRules?: string[];
  hasRemarks?: boolean;
  hasChorusProRemarks?: boolean;
  children?: CiiSummaryNode[];
}
