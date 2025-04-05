import { Component, computed, inject, input } from '@angular/core';
import { ScrollToDirective } from '../../../../core/scroll-to/scroll-to.directive';
import { EditorSettings } from '../../editor-settings.service';
import { CiiFormHighlightTermService } from '../../services/cii-form-highlight-term.service';
import { CiiFormHighlightBusinessRuleService } from '../../services/cii-form-highlight-business-rule.service';
import { CiiFormHighlightRemarkService } from '../../services/cii-form-highlight-remark.service';
import { CiiFormHighlightChorusProRemarkService } from '../../services/cii-form-highlight-chorus-pro-remark.service';
import { CiiFormControl } from '../cii-form/cii-form.service';

@Component({
  selector: 'app-cii-summary-node',
  imports: [ScrollToDirective],
  template: `
    <div class="text-truncate d-flex gap-1">
      @if (node().kind === 'group') {
        <span [class.fw-bold]="isNodeHighlighted()" (mouseenter)="this.highlightTerm(true)" (mouseleave)="this.highlightTerm(false)">
          <span class="fw-semibold"> {{ node().term }} </span> - {{ node().name }}
        </span>
      } @else {
        <a
          [scrollTo]="node().term"
          [class.fw-bold]="isNodeHighlighted()"
          [class.text-success]="isValid"
          [class.text-danger]="isInvalid"
          data-bs-dismiss="offcanvas"
          data-bs-target="#editor__cii-summary"
          (mouseenter)="this.highlightTerm(true)"
          (mouseleave)="this.highlightTerm(false)"
        >
          <span class="fw-semibold"> {{ node().term }} </span> - {{ node().name }}
        </a>
      }

      @if (settings(); as settings) {
        <div class="text-body-tertiary d-flex align-items-center gap-1" style="font-size: smaller">
          @if (settings.showBusinessRules) {
            @if (node().businessRules; as businessRules) {
              @for (rule of businessRules; track rule) {
                <a
                  [scrollTo]="rule"
                  class="hoverlink"
                  [class.fw-bold]="highlightedBusinessRule() === rule"
                  (mouseenter)="highlightBusinessRule(rule, true)"
                  (mouseleave)="highlightBusinessRule(rule, false)"
                  >{{ rule }}</a
                >
              }
            }
          }

          @if (node().hasRemarks && settings.showRemarks) {
            <a
              [scrollTo]="node().term + '-remarks'"
              class="hoverlink"
              [class.fw-bold]="highlightedRemark() === node().term"
              (mouseenter)="highlightRemark(node().term, true)"
              (mouseleave)="highlightRemark(node().term, false)"
            >
              <i class="bi bi-info-circle"></i>
            </a>
          }

          @if (node().hasChorusProRemarks && settings.showChorusProRemarks) {
            <a
              [scrollTo]="node().term + '-cpro-remarks'"
              class="hoverlink"
              [class.fw-bold]="highlightedChorusProRemark() === node().term"
              (mouseenter)="highlightChorusProRemark(node().term, true)"
              (mouseleave)="highlightChorusProRemark(node().term, false)"
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
  `,
})
export class CiiSummaryControlComponent {
  node = input.required<CiiFormControl>();
  settings = input.required<EditorSettings>();

  protected get isValid(): boolean {
    const node = this.node();
    return node.control.touched && !node.control.invalid;
  }

  protected get isInvalid(): boolean {
    const node = this.node();
    return node.control.touched && node.control.invalid;
  }

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

  protected highlightRemark(rule: string, value: boolean) {
    this.highlightRemarkService.highlightRemark(rule, value);
  }

  protected highlightChorusProRemark(rule: string, value: boolean) {
    this.highlightChorusProRemarkService.highlightChorusProRemark(rule, value);
  }

  protected highlightBusinessRule(rule: string, value: boolean) {
    this.highlightBusinessRuleService.highlightBusinessRule(rule, value);
  }
}
