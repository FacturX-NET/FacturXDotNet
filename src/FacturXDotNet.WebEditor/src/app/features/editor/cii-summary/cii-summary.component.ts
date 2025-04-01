import { Component, input } from '@angular/core';
import { CrossIndustryInvoice } from '../../../core/facturx-models/cii/cross-industry-invoice';
import { CiiSummaryNodeComponent } from './cii-summary-node.component';
import { EditorSettings } from '../editor-settings.service';

@Component({
  selector: 'app-cii-summary',
  imports: [CiiSummaryNodeComponent],
  template: `
    <div id="editor__cii-summary-content">
      @for (node of menus; track node.code) {
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
      code: 'BG-2',
      name: 'EXCHANGE DOCUMENT CONTEXT',
      children: [
        {
          code: 'BT-23',
          name: 'Business process type',
        },
        {
          code: 'BT-24',
          name: 'Specification identifier',
          businessRules: ['BR-1'],
          hasRemarks: true,
        },
      ],
    },
    {
      code: 'BT-1-00',
      name: 'EXCHANGED DOCUMENT',
      children: [
        {
          code: 'BT-1',
          name: 'Invoice number',
          businessRules: ['BR-2'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          code: 'BT-3',
          name: 'Invoice type code',
          businessRules: ['BR-4'],
          hasRemarks: true,
          hasChorusProRemarks: true,
        },
        {
          code: 'BT-2',
          name: 'Invoice issue date',
          businessRules: ['BR-3'],
          hasChorusProRemarks: true,
          children: [
            {
              code: 'BT-2-0',
              name: 'Date, format',
            },
          ],
        },
      ],
    },
    {
      code: 'BG-25-00',
      name: 'SUPPLY CHAIN TRADE TRANSACTION',
      children: [
        {
          code: 'BT-10-00',
          name: 'HEADER TRADE AGREEMENT',
          children: [
            {
              code: 'BT-10',
              name: 'Buyer reference',
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              code: 'BG-4',
              name: 'SELLER',
              children: [
                {
                  code: 'BT-27',
                  name: 'Seller name',
                  businessRules: ['BR-6'],
                },
                {
                  code: 'BT-30-00',
                  name: 'SELLER LEGAL ORGANIZATION',
                  children: [
                    {
                      code: 'BT-30',
                      name: 'Seller legal registration identifier',
                      businessRules: ['BR-CO-26'],
                      children: [
                        {
                          code: 'BT-30-1',
                          name: 'Scheme identifier',
                          hasRemarks: true,
                        },
                      ],
                    },
                  ],
                },
                {
                  code: 'BG-5',
                  name: 'SELLER POSTAL ADDRESS',
                  businessRules: ['BR-8'],
                  hasRemarks: true,
                  children: [
                    {
                      code: 'BT-40',
                      name: 'Seller country code',
                      businessRules: ['BR-9'],
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  code: 'BT-31-00',
                  name: 'SELLER VAT IDENTIFIER',
                  children: [
                    {
                      code: 'BT-31',
                      name: 'Seller VAT identifier',
                      businessRules: ['BR-CO-9', 'BR-CO-26'],
                      hasRemarks: true,
                      children: [
                        {
                          code: 'BT-31-0',
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
              code: 'BG-7',
              name: 'BUYER',
              children: [
                {
                  code: 'BT-44',
                  name: 'Buyer name',
                  businessRules: ['BR-7'],
                },
                {
                  code: 'BT-47-00',
                  name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
                  children: [
                    {
                      code: 'BT-47',
                      name: 'Buyer legal registration identifier',
                      hasRemarks: true,
                      hasChorusProRemarks: true,
                      children: [
                        {
                          code: 'BT-47-1',
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
              code: 'BT-13-00',
              name: 'PURCHASE ORDER REFERENCE',
              children: [
                {
                  code: 'BT-13',
                  name: 'Purchase order reference',
                  hasChorusProRemarks: true,
                },
              ],
            },
          ],
        },
        {
          code: 'BG-13-00',
          name: 'DELIVERY INFORMATION',
        },
        {
          code: 'BG-19',
          name: 'HEADER TRADE SETTLEMENT DIRECT DEBIT',
          hasRemarks: true,
          hasChorusProRemarks: true,
          children: [
            {
              code: 'BT-5',
              name: 'Invoice currency code',
              businessRules: ['BR-5'],
              hasRemarks: true,
              hasChorusProRemarks: true,
            },
            {
              code: 'BG-22',
              name: 'DOCUMENT TOTALS',
              hasRemarks: true,
              hasChorusProRemarks: true,
              children: [
                {
                  code: 'BT-109',
                  name: 'Total amount without VAT',
                  businessRules: ['BR-13', 'BR-CO-13'],
                  hasRemarks: true,
                },
                {
                  code: 'BT-110',
                  name: 'Total VAT amount',
                  businessRules: ['BR-CO-14'],
                  hasRemarks: true,
                  children: [
                    {
                      code: 'BT-110-1',
                      name: 'VAT currency',
                      hasRemarks: true,
                    },
                  ],
                },
                {
                  code: 'BT-112',
                  name: 'Total amount with VAT',
                  businessRules: ['BR-14', 'BR-CO-15', 'BR-FXEXT-CO-15'],
                  hasRemarks: true,
                },
                {
                  code: 'BT-115',
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
  code: string;
  name: string;
  businessRules?: string[];
  hasRemarks?: boolean;
  hasChorusProRemarks?: boolean;
  children?: CiiSummaryNode[];
}
