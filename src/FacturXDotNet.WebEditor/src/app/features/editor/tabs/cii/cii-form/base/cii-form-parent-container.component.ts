import { Component, computed, ElementRef, inject, input, numberAttribute, Signal, viewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CiiFormRemarkComponent } from './cii-form-remark.component';
import { CiiFormBusinessRulesComponent } from './cii-form-business-rules.component';
import { EditorSettings } from '../../../../editor-settings.service';
import { CiiFormHighlightTermService } from '../../cii-form-highlight-term.service';
import { CiiFormHighlightRemarkService } from '../../cii-form-highlight-remark.service';
import { CiiFormHighlightChorusProRemarkService } from '../../cii-form-highlight-chorus-pro-remark.service';
import { CiiTerm } from '../../constants/cii-terms';
import { BusinessRule, requireRule } from '../../constants/cii-business-rules';
import { MarkdownComponent } from 'ngx-markdown';

@Component({
  selector: 'app-cii-form-parent-container',
  imports: [ReactiveFormsModule, CiiFormRemarkComponent, CiiFormBusinessRulesComponent, MarkdownComponent],
  template: `
    <h6 #title [id]="term()" class="py-2 m-0 sticky-top bg-body" style="top: {{ topPx() }}px; z-index: {{ zIndex() }}" [class.text-primary]="isTermHighlighted()">
      {{ term().term }} - {{ term().name }}
    </h6>

    <div class="ps-3 border-start border-2" [class.border-primary]="isTermHighlighted()">
      @if (term().description !== undefined) {
        <p class="form-text" [class.text-primary]="isTermHighlighted()">
          <markdown ngPreserveWhitespaces>{{ term().description }}</markdown>
        </p>
      }

      <div class="pb-2"><!--spacer--></div>

      @if (settings(); as settings) {
        @if (businessRules().length > 0) {
          @if (settings?.showBusinessRules === true) {
            <div [id]="businessRulesId()">
              <app-cii-form-business-rules [businessRules]="businessRules()" [highlight]="isTermHighlighted()"></app-cii-form-business-rules>
            </div>
          }
        }

        @if (term().remark; as remark) {
          @if (settings.showRemarks) {
            <div [id]="remarkId()">
              <app-cii-form-remark [remark]="remark" [highlight]="isTermHighlighted() || isRemarkHighlighted()" />
            </div>
          }
        }

        @if (term().chorusProRemark; as chorusProRemark) {
          @if (settings.showChorusProRemarks) {
            <div [id]="chorusProRemarkId()">
              <app-cii-form-remark [remark]="chorusProRemark" title="CHORUSPRO" [highlight]="isTermHighlighted() || isChorusProRemarkHighlighted()" />
            </div>
          }
        }
      }
      <div class="ps-1">
        <ng-content></ng-content>
      </div>
    </div>
  `,
})
export class CiiFormParentContainerComponent {
  term = input.required<CiiTerm>();
  settings = input<EditorSettings>();
  depth = input.required({ transform: numberAttribute });

  public businessRulesId = computed(() => this.term().term + '-rules');
  public remarkId = computed(() => this.term().term + '-remarks');
  public chorusProRemarkId = computed(() => this.term().term + '-cpro-remarks');

  public businessRules: Signal<BusinessRule[]> = computed(() => {
    const ruleNames = this.term().businessRules;
    if (ruleNames === undefined) {
      return [];
    }

    return ruleNames.map((r) => requireRule(r));
  });

  protected topPx = computed(() => {
    const depth = this.depth();
    const element = this.titleElement()?.nativeElement as HTMLElement;

    return (depth - 1) * (element.offsetHeight + 16);
  });

  protected zIndex = computed(() => {
    const depth = this.depth();
    return 1000 - depth;
  });

  private titleElement = viewChild<ElementRef>('title');

  private highlightService = inject(CiiFormHighlightTermService);
  protected isTermHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term().term);

  private remarkHighlightService = inject(CiiFormHighlightRemarkService);
  protected isRemarkHighlighted = computed(() => this.remarkHighlightService.highlightedRemark() === this.term().term);

  private chorusProRemarkHighlightService = inject(CiiFormHighlightChorusProRemarkService);
  protected isChorusProRemarkHighlighted = computed(() => this.chorusProRemarkHighlightService.highlightedChorusProRemark() === this.term().term);
}
