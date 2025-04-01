import { Component, inject, input } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent } from './cii-form-buyer-trade-party-specified-legal-organization.component';
import { EditorSettings } from '../editor-settings.service';
import { CiiFormParentContainerComponent } from './components/cii-form-parent-container.component';
import { CiiFormSpecifiedTradeSettlementHeaderMonetarySummation } from './cii-form-specified-trade-settlement-header-monetary-summation.component';

@Component({
  selector: 'app-cii-form-buyer-trade-party',
  imports: [ReactiveFormsModule, CiiFormBuyerTradePartySpecifiedLegalOrganizationComponent, CiiFormParentContainerComponent],
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
      <div>
        <div class="editor__control">
          <label class="form-label text-truncate" for="buyerName"> <span id="BT-44" class="fw-semibold">BT-44</span> - Buyer name </label>
          <input id="buyerName" class="form-control" formControlName="name" placeholder="LE CLIENT" />
          <p id="buyerNameHelp" class="form-text">The full name of the Buyer.</p>
        </div>
        @if (settings()?.showBusinessRules === true) {
          <div class="form-text">
            <div class="fw-semibold">Business Rules</div>
            <ul>
              <li><span id="BR-7" class="fw-semibold">BR-7</span>: An Invoice shall contain the Buyer name.</li>
            </ul>
          </div>
        }
      </div>

      <app-cii-form-parent-container term="BT-47-00" name="BUYER LEGAL REGISTRATION IDENTIFIER" [description]="description" [settings]="settings()">
        <ng-template #description>Details about the organization.</ng-template>

        <app-cii-form-buyer-trade-party-specified-legal-organization
          formGroupName="specifiedLegalOrganization"
          [settings]="settings()"
        ></app-cii-form-buyer-trade-party-specified-legal-organization>
      </app-cii-form-parent-container>
    </div>
  `,
  styles: ``,
})
export class CiiFormBuyerTradePartyComponent {
  formGroupName = input.required<string>();
  settings = input<EditorSettings>();
}
