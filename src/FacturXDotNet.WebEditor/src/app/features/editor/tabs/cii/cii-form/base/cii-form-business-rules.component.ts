import { Component, computed, effect, inject, input, TemplateRef } from '@angular/core';
import { CiiFormHighlightBusinessRuleService } from '../../cii-form-highlight-business-rule.service';
import { CiiFormService } from '../cii-form.service';
import { CiiRule } from '../../constants/cii-rules';
import { MarkdownComponent, MarkdownService } from 'ngx-markdown';
import { CiiFormBusinessRuleStatusIconComponent } from './cii-form-business-rule-status-icon.component';

@Component({
  selector: 'app-cii-form-business-rules',
  imports: [MarkdownComponent, CiiFormBusinessRuleStatusIconComponent],
  template: `
    <div class="form-text" [class.text-primary]="highlight()">
      <div class="fw-semibold">Business Rules</div>
      <div class="ps-2 pb-2">
        @for (rule of businessRules(); track rule.id) {
          <div
            class="d-flex gap-1"
            [id]="rule.id"
            [class.text-primary]="highlightedRule() === rule.id && statuses()[rule.id] !== 'invalid' && statuses()[rule.id] !== 'valid'"
            [class.text-danger]="statuses()[rule.id] === 'invalid'"
            [class.text-success]="statuses()[rule.id] === 'valid'"
          >
            <app-cii-form-business-rule-status-icon [highlighted]="highlightedRule() === rule.id" [status]="statuses()[rule.id]" />
            <span [id]="rule.id" class="fw-semibold text-nowrap" [class.fw-bold]="highlight() || highlightedRule() == rule.id">{{ rule.id }} </span>:
            <markdown ngPreserveWhitespaces>{{ rule.description }}</markdown>
          </div>
        }
      </div>
    </div>
  `,
})
export class CiiFormBusinessRulesComponent {
  businessRules = input.required<CiiRule[]>();
  highlight = input<boolean>(false);

  private highlightService = inject(CiiFormHighlightBusinessRuleService);
  protected highlightedRule = this.highlightService.highlightedBusinessRule;

  private ciiFormService = inject(CiiFormService);
  protected statuses = this.ciiFormService.businessRulesValidation;
}
