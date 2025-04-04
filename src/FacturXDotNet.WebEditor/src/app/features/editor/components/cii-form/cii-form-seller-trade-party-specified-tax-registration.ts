import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormControlComponent } from './base/cii-form-control.component';
import { ScrollToDirective } from '../../../../core/scroll-to/scroll-to.directive';

@Component({
  selector: 'app-cii-form-seller-trade-party-specified-tax-registration',
  imports: [ReactiveFormsModule, CiiFormControlComponent, ScrollToDirective],
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
      <div class="row">
        <div class="col">
          <app-cii-form-control
            term="BT-31"
            name="Seller VAT identifier"
            [description]="bt31Description"
            [businessRules]="[
              { id: 'BR-CO-9', template: brCo9 },
              { id: 'BR-CO-26', template: brCo26 },
            ]"
            [remarks]="[bt31Remark]"
            [settings]="settings()"
            #bt31Control
          >
            <ng-template #bt31Description>The Seller's VAT identifier (also known as Seller VAT identification number).</ng-template>
            <ng-template #brCo9>
              The Seller VAT identifier, the <a [scrollTo]="'BT-63'">Seller tax representative VAT identifier (BT-63)</a> and the
              <a [scrollTo]="'BT-48'">Buyer VAT identifier (BT-48)</a> shall have a prefix in accordance with ISO code ISO 3166-1 alpha-2 by which the country of issue may be
              identified. Nevertheless, Greece may use the prefix ‘EL’.
            </ng-template>
            <ng-template #brCo26>
              In order for the buyer to automatically identify a supplier, the <a [scrollTo]="'BT-29'">Seller identifier (BT-29)</a>, the
              <a [scrollTo]="'BT-30'">Seller legal registration identifier (BT-30)</a> and/or the Seller VAT identifier shall be present.
            </ng-template>
            <ng-template #bt31Remark>
              VAT number prefixed by a country code. A VAT registered Supplier shall include his VAT ID, except when he uses a tax representative.
            </ng-template>

            <input [id]="bt31Control.controlId()" class="form-control" formControlName="id" placeholder="FR11123456782" />
          </app-cii-form-control>
        </div>

        <div class="col">
          <app-cii-form-control term="BT-31-0" name="Tax Scheme identifier" [description]="bt310Description" [remarks]="[bt310Remark]" [settings]="settings()" #bt310Control>
            <ng-template #bt310Description>Scheme identifier for supplier VAT identifier.</ng-template>
            <ng-template #bt310Remark>Value = VA</ng-template>

            <select [id]="bt310Control.controlId()" class="form-select" formControlName="idSchemeId">
              <option value="Vat" selected>VAT</option>
            </select>
          </app-cii-form-control>
        </div>
      </div>
    </div>
  `,
})
export class CiiFormSellerTradePartySpecifiedTaxRegistration {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
