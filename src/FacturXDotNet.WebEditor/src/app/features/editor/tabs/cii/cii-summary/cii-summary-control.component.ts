import { Component, computed, inject, input } from '@angular/core';
import { CiiFormControl, CiiFormService } from '../cii-form/cii-form.service';
import { EditorSettings } from '../../../editor-settings.service';
import { ScrollToDirective } from '../../../../../core/scroll-to/scroll-to.directive';
import { CiiFormHighlightChorusProRemarkService } from '../cii-form-highlight-chorus-pro-remark.service';
import { CiiFormHighlightTermService } from '../cii-form-highlight-term.service';
import { CiiFormHighlightRemarkService } from '../cii-form-highlight-remark.service';
import { CiiFormHighlightBusinessRuleService } from '../cii-form-highlight-business-rule.service';
import { BusinessTermIdentifier, getTerm } from '../constants/cii-terms';
import { BusinessRuleIdentifier } from '../constants/cii-business-rules';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    @if (term(); as term) {
      <div class="text-truncate d-flex gap-1">
        @if (term.kind === 'group') {
          <span [class.fw-bold]="isNodeHighlighted()" (mouseenter)="this.highlightTerm(true)" (mouseleave)="this.highlightTerm(false)">
            <span class="fw-semibold"> {{ term.term }} </span> - {{ term.name }}
          </span>
        } @else {
          <a
            [scrollTo]="term.term"
            [class.fw-bold]="isNodeHighlighted()"
            [class.text-success]="isValid"
            [class.text-danger]="isInvalid"
            data-bs-dismiss="offcanvas"
            data-bs-target="#editor__cii-summary"
            (mouseenter)="this.highlightTerm(true)"
            (mouseleave)="this.highlightTerm(false)"
          >
            <span class="fw-semibold"> {{ term.term }} </span> - {{ term.name }}
          </a>
        }

        @if (settings(); as settings) {
          <div class="text-body-tertiary d-flex align-items-center gap-1" style="font-size: smaller">
            @if (settings.showBusinessRules) {
              @if (term.businessRules; as businessRules) {
                @for (rule of businessRules; track rule) {
                  <a
                    [scrollTo]="rule"
                    class="hoverlink"
                    [class.fw-bold]="highlightedBusinessRule() === rule"
                    [class.text-danger]="businessRuleStatuses()[rule] === 'invalid'"
                    [class.text-success]="businessRuleStatuses()[rule] === 'valid'"
                    (mouseenter)="highlightBusinessRule(rule, true)"
                    (mouseleave)="highlightBusinessRule(rule, false)"
                    >{{ rule }}</a
                  >
                }
              }
            }

            @if (term.remark !== undefined && settings.showRemarks) {
              <a
                [scrollTo]="term.term + '-remarks'"
                class="hoverlink"
                [class.fw-bold]="highlightedRemark() === term.term"
                (mouseenter)="highlightRemark(term.term, true)"
                (mouseleave)="highlightRemark(term.term, false)"
              >
                <i class="bi bi-info-circle"></i>
              </a>
            }

            @if (term.chorusProRemark !== undefined && settings.showChorusProRemarks) {
              <a
                [scrollTo]="term.term + '-cpro-remarks'"
                class="hoverlink"
                [class.fw-bold]="highlightedChorusProRemark() === term.term"
                (mouseenter)="highlightChorusProRemark(term.term, true)"
                (mouseleave)="highlightChorusProRemark(term.term, false)"
              >
                CPRO
              </a>
            }
          </div>
        }
      </div>
      <div class="border-start ps-2">
        @if (node().children; as children) {
          @for (child of node().children; track child.term) {
            <app-cii-summary-node [node]="child" [settings]="settings()" />
          }
        }
      </div>
    }
  `,
})
export class CiiSummaryControlComponent {
  node = input.required<CiiFormControl>();
  settings = input.required<EditorSettings>();

  protected term = computed(() => getTerm(this.node().term));

  protected get isValid(): boolean {
    const node = this.node();
    return node.control.touched && !node.control.invalid;
  }

  protected get isInvalid(): boolean {
    const node = this.node();
    return node.control.touched && node.control.invalid;
  }

  private ciiFormService = inject(CiiFormService);
  protected businessRuleStatuses = this.ciiFormService.businessRules;

  private highlightTermService = inject(CiiFormHighlightTermService);
  protected isNodeHighlighted = computed(() => this.highlightTermService.highlightedTerm() === this.node().term);

  private highlightRemarkService = inject(CiiFormHighlightRemarkService);
  protected highlightedRemark = this.highlightRemarkService.highlightedRemark;

  private highlightChorusProRemarkService = inject(CiiFormHighlightChorusProRemarkService);
  protected highlightedChorusProRemark = this.highlightChorusProRemarkService.highlightedChorusProRemark;

  private highlightBusinessRuleService = inject(CiiFormHighlightBusinessRuleService);
  protected highlightedBusinessRule = this.highlightBusinessRuleService.highlightedBusinessRule;

  protected highlightTerm(value: boolean) {
    this.highlightTermService.highlightTerm(this.node().term, value);
  }

  protected highlightRemark(rule: BusinessTermIdentifier, value: boolean) {
    this.highlightRemarkService.highlightRemark(rule, value);
  }

  protected highlightChorusProRemark(rule: BusinessTermIdentifier, value: boolean) {
    this.highlightChorusProRemarkService.highlightChorusProRemark(rule, value);
  }

  protected highlightBusinessRule(rule: BusinessRuleIdentifier, value: boolean) {
    this.highlightBusinessRuleService.highlightBusinessRule(rule, value);
  }
}
