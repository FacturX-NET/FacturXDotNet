import { Component, input } from '@angular/core';
import { CrossIndustryInvoice } from '../../../core/facturx-models/cii/cross-industry-invoice';
import { CiiSummaryNodeComponent } from './cii-summary-node.component';

@Component({
  selector: 'app-cii-summary',
  imports: [CiiSummaryNodeComponent],
  template: `
    <div id="editor__cii-summary-content">
      @for (node of menus; track node.code) {
        <app-cii-summary-node [node]="node" />
      }
    </div>
  `,
})
export class CiiSummaryComponent {
  value = input.required<CrossIndustryInvoice>();

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
        },
        {
          code: 'BT-3',
          name: 'Invoice type code',
        },
        {
          code: 'BT-2',
          name: 'Invoice issue date',
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
            },
            {
              code: 'BG-4',
              name: 'SELLER',
              children: [
                {
                  code: 'BT-27',
                  name: 'Seller name',
                },
                {
                  code: 'BT-30-00',
                  name: 'SELLER LEGAL ORGANIZATION',
                  children: [
                    {
                      code: 'BT-30',
                      name: 'Seller legal registration identifier',
                      children: [
                        {
                          code: 'BT-30-1',
                          name: 'Scheme identifier',
                        },
                      ],
                    },
                  ],
                },
                {
                  code: 'BG-5',
                  name: 'SELLER POSTAL ADDRESS',
                  children: [
                    {
                      code: 'BT-40',
                      name: 'Seller country code',
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
                      children: [
                        {
                          code: 'BT-31-0',
                          name: 'Tax Scheme identifier',
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
                },
                {
                  code: 'BT-47-00',
                  name: 'BUYER LEGAL REGISTRATION IDENTIFIER',
                  children: [
                    {
                      code: 'BT-47',
                      name: 'Buyer legal registration identifier',
                      children: [
                        {
                          code: 'BT-47-1',
                          name: 'Scheme identifier',
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
          children: [
            {
              code: 'BT-5',
              name: 'Invoice currency code',
            },
            {
              code: 'BG-22',
              name: 'DOCUMENT TOTALS',
              children: [
                {
                  code: 'BT-109',
                  name: 'Total amount without VAT',
                },
                {
                  code: 'BT-110',
                  name: 'Total VAT amount',
                  children: [
                    {
                      code: 'BT-110-1',
                      name: 'VAT currency',
                    },
                  ],
                },
                {
                  code: 'BT-112',
                  name: 'Total amount with VAT',
                },
                {
                  code: 'BT-115',
                  name: 'Amount due for payment',
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
  children?: CiiSummaryNode[];
}
