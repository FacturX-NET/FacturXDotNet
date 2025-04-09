import { Component, computed, inject, input, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { CiiFormHighlightBusinessRuleService } from '../../cii-form-highlight-business-rule.service';
import { CiiFormService } from '../cii-form.service';

@Component({
  selector: 'app-cii-form-business-rules',
  imports: [NgTemplateOutlet],
  template: `
    <div class="form-text" [class.text-primary]="highlight()">
      <div class="fw-semibold">Business Rules</div>
      <div class="ps-2 pb-2">
        @for (rule of businessRules(); track rule.id) {
          <div
            [class.text-primary]="highlightedRule() === rule.id"
            [class.text-danger]="highlightedRule() !== rule.id && statuses()[rule.id] == 'invalid'"
            [class.text-success]="highlightedRule() !== rule.id && statuses()[rule.id] == 'valid'"
          >
            <span class="pe-1">
              @if (highlightedRule() == rule.id) {
                <i class="bi bi-arrow-right"></i>
              } @else {
                @switch (statuses()[rule.id]) {
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

            <span [id]="rule.id" class="fw-semibold" [class.fw-bold]="highlight() || highlightedRule() == rule.id">{{ rule.id }} </span>:
            <ng-container [ngTemplateOutlet]="rule.template"></ng-container>
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormBusinessRulesComponent {
  businessRules = input.required<BusinessRuleTemplate[]>();
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
