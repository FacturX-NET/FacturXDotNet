import { Component, inject, input, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { CiiFormHighlightBusinessRuleService } from '../../../services/cii-form-highlight-business-rule.service';

@Component({
  selector: 'app-cii-form-business-rules',
  imports: [NgTemplateOutlet],
  template: `
    <div class="form-text" [class.text-primary]="highlight()">
      <div class="fw-semibold">Business Rules</div>
      <ul>
        @for (rule of businessRules(); track rule.id) {
          <li [class.text-primary]="highlightedRule() == rule.id">
            <span [id]="rule.id" class="fw-semibold" [class.fw-bold]="highlight() || highlightedRule() == rule.id">{{ rule.id }} </span>:
            <ng-container [ngTemplateOutlet]="rule.template"></ng-container>
          </li>
        }
      </ul>
    </div>
  `,
})
export class CiiFormBusinessRulesComponent {
  businessRules = input.required<BusinessRuleTemplate[]>();
  highlight = input<boolean>(false);

  private highlightService = inject(CiiFormHighlightBusinessRuleService);
  protected highlightedRule = this.highlightService.highlightedBusinessRule;
}

export interface BusinessRuleTemplate {
  readonly id: string;
  readonly template: TemplateRef<any>;
}
