import { Component, computed, inject, input, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { CiiFormHighlightBusinessRuleService } from '../../cii-form-highlight-business-rule.service';
import { CiiFormService } from '../cii-form.service';
import { BusinessRule } from '../../constants/cii-business-rules';

@Component({
  selector: 'app-cii-form-business-rules',
  imports: [NgTemplateOutlet],
  template: `
    <div class="form-text" [class.text-primary]="highlight()">
      <div class="fw-semibold">Business Rules</div>
      <div class="ps-2 pb-2">
        @for (rule of businessRules(); track rule.name) {
          <div
            [class.text-primary]="highlightedRule() === rule.name && statuses()[rule.name] !== 'invalid' && statuses()[rule.name] !== 'valid'"
            [class.text-danger]="statuses()[rule.name] === 'invalid'"
            [class.text-success]="statuses()[rule.name] === 'valid'"
          >
            <span class="pe-1">
              @if (highlightedRule() === rule.name && statuses()[rule.name] !== 'invalid' && statuses()[rule.name] !== 'valid') {
                <i class="bi bi-arrow-right"></i>
              } @else {
                @switch (statuses()[rule.name]) {
                  @case ('valid') {
                    <i class="bi bi-check-circle-fill"></i>
                  }
                  @case ('invalid') {
                    <i class="bi bi-x-circle-fill"></i>
                  }
                  @default {
                    <i class="bi bi-dash"></i>
                  }
                }
              }
            </span>

            <span [id]="rule.name" class="fw-semibold" [class.fw-bold]="highlight() || highlightedRule() == rule.name">{{ rule.name }} </span>:
            {{ rule.description }}
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormBusinessRulesComponent {
  businessRules = input.required<BusinessRule[]>();
  highlight = input<boolean>(false);

  private highlightService = inject(CiiFormHighlightBusinessRuleService);
  protected highlightedRule = this.highlightService.highlightedBusinessRule;

  private ciiFormService = inject(CiiFormService);
  protected statuses = this.ciiFormService.businessRules;
}

export interface BusinessRuleTemplate {
  readonly id: string;
  readonly template: TemplateRef<any>;
}
