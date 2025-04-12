import { Component, computed, effect, inject, input, TemplateRef } from '@angular/core';
import { CiiFormHighlightBusinessRuleService } from '../../cii-form-highlight-business-rule.service';
import { CiiFormService } from '../cii-form.service';
import { BusinessRule } from '../../constants/cii-business-rules';
import { MarkdownComponent, MarkdownService } from 'ngx-markdown';
import { CiiFormBusinessRuleStatusIconComponent } from './cii-form-business-rule-status-icon.component';

@Component({
  selector: 'app-cii-form-business-rules',
  imports: [MarkdownComponent, CiiFormBusinessRuleStatusIconComponent],
  template: `
    <div class="form-text" [class.text-primary]="highlight()">
      <div class="fw-semibold">Business Rules</div>
      <div class="ps-2 pb-2">
        @for (rule of businessRules(); track rule.name) {
          <div
            class="d-flex gap-1"
            [id]="rule.name"
            [class.text-primary]="highlightedRule() === rule.name && statuses()[rule.name] !== 'invalid' && statuses()[rule.name] !== 'valid'"
            [class.text-danger]="statuses()[rule.name] === 'invalid'"
            [class.text-success]="statuses()[rule.name] === 'valid'"
          >
            <app-cii-form-business-rule-status-icon [highlighted]="highlightedRule() === rule.name" [status]="statuses()[rule.name]" />
            <span [id]="rule.name" class="fw-semibold text-nowrap" [class.fw-bold]="highlight() || highlightedRule() == rule.name">{{ rule.name }} </span>:
            <markdown ngPreserveWhitespaces>{{ rule.description }}</markdown>
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
