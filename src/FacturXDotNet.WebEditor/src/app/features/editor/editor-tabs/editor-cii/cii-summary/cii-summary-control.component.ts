import { Component, computed, inject, input } from '@angular/core';
import { CiiFormNode, CiiFormService } from '../cii-form/cii-form.service';
import { EditorSettings } from '../../../services/editor-settings.service';
import { ScrollToDirective } from '../../../../../core/scroll-to/scroll-to.directive';
import { CiiFormHighlightChorusProRemarkService } from '../cii-form-highlight-chorus-pro-remark.service';
import { CiiFormHighlightTermService } from '../cii-form-highlight-term.service';
import { CiiFormHighlightRemarkService } from '../cii-form-highlight-remark.service';
import { CiiFormHighlightBusinessRuleService } from '../cii-form-highlight-business-rule.service';
import { BusinessRuleIdentifier } from '../constants/cii-rules';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    @if (node(); as node) {
      <div class="text-truncate d-flex gap-1">
        @if (node.term.kind === 'group') {
          <span [class.fw-bold]="isNodeHighlighted()" (mouseenter)="this.highlightTerm(true)" (mouseleave)="this.highlightTerm(false)">
            <span class="fw-semibold"> {{ node.term.id }} </span> - {{ node.term.name }}
          </span>
        } @else {
          <a
            [scrollTo]="node.term.id"
            [class.fw-bold]="isNodeHighlighted()"
            [class.text-success]="isValid()"
            [class.text-danger]="isInvalid()"
            data-bs-dismiss="offcanvas"
            data-bs-target="#editor__cii-summary"
            (mouseenter)="this.highlightTerm(true)"
            (mouseleave)="this.highlightTerm(false)"
          >
            <span class="fw-semibold"> {{ node.term.id }} </span> - {{ node.term.name }}
          </a>
        }

        @if (settings(); as settings) {
          <div class="text-body-tertiary d-flex align-items-center gap-1" style="font-size: smaller">
            @if (settings.showBusinessRules) {
              @if (node.term.businessRules; as businessRules) {
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

            @if (node.term.remark !== undefined && settings.showRemarks) {
              <a
                [scrollTo]="node.term.id + '-remarks'"
                class="hoverlink"
                [class.fw-bold]="highlightedRemark() === node.term.id"
                (mouseenter)="highlightRemark(true)"
                (mouseleave)="highlightRemark(false)"
              >
                <i class="bi bi-info-circle"></i>
              </a>
            }

            @if (node.term.chorusProRemark !== undefined && settings.showChorusProRemarks) {
              <a
                [scrollTo]="node.term.id + '-cpro-remarks'"
                class="hoverlink"
                [class.fw-bold]="highlightedChorusProRemark() === node.term.id"
                (mouseenter)="highlightChorusProRemark(true)"
                (mouseleave)="highlightChorusProRemark(false)"
              >
                CPRO
              </a>
            }
          </div>
        }
      </div>
      <div class="border-start ps-2">
        @if (node.children; as children) {
          @for (child of node.children; track child.term) {
            <app-cii-summary-node [node]="child" [settings]="settings()" />
          }
        }
      </div>
    }
  `,
})
export class CiiSummaryControlComponent {
  node = input.required<CiiFormNode>();
  settings = input.required<EditorSettings>();

  private ciiFormService = inject(CiiFormService);
  protected businessRuleStatuses = this.ciiFormService.businessRulesValidation;
  private termValidation = this.ciiFormService.businessTermsValidation;

  protected isValid = computed(() => {
    const node = this.node();
    const validation = this.termValidation()[node.term.id];

    return validation === 'valid';
  });

  protected isInvalid = computed(() => {
    const node = this.node();
    const validation = this.termValidation()[node.term.id];

    return validation === 'invalid';
  });

  private highlightTermService = inject(CiiFormHighlightTermService);
  protected isNodeHighlighted = computed(() => this.highlightTermService.highlightedTerm() === this.node().term.id);

  private highlightRemarkService = inject(CiiFormHighlightRemarkService);
  protected highlightedRemark = this.highlightRemarkService.highlightedRemark;

  private highlightChorusProRemarkService = inject(CiiFormHighlightChorusProRemarkService);
  protected highlightedChorusProRemark = this.highlightChorusProRemarkService.highlightedChorusProRemark;

  private highlightBusinessRuleService = inject(CiiFormHighlightBusinessRuleService);
  protected highlightedBusinessRule = this.highlightBusinessRuleService.highlightedBusinessRule;

  protected highlightTerm(value: boolean) {
    this.highlightTermService.highlightTerm(this.node().term.id, value);
  }

  protected highlightRemark(value: boolean) {
    this.highlightRemarkService.highlightRemark(this.node().term.id, value);
  }

  protected highlightChorusProRemark(value: boolean) {
    this.highlightChorusProRemarkService.highlightChorusProRemark(this.node().term.id, value);
  }

  protected highlightBusinessRule(rule: BusinessRuleIdentifier, value: boolean) {
    this.highlightBusinessRuleService.highlightBusinessRule(rule, value);
  }
}
