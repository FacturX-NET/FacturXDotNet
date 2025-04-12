import { Component, computed, inject, input, Signal } from '@angular/core';
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
  selector: 'app-cii-form-control',
  imports: [ReactiveFormsModule, CiiFormRemarkComponent, CiiFormBusinessRulesComponent, MarkdownComponent],
  template: `
    <div class="overflow-hidden px-1">
      <div class="editor__control">
        <label [id]="id()" class="form-label text-truncate" [class.text-primary]="isTermHighlighted()" [for]="controlId()">
          <span class="fw-semibold">{{ term().term }}</span> - {{ term().name }}
        </label>

        <ng-content></ng-content>
      </div>

      @if (term().description !== undefined) {
        <p [id]="controlHelpId()" class="form-text" [class.text-primary]="isTermHighlighted()">
          <markdown ngPreserveWhitespaces>{{ term().description }}</markdown>
        </p>
      } @else {
        <div class="pb-3"><!--spacer--></div>
      }

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
    </div>
  `,
})
export class CiiFormControlComponent {
  term = input.required<CiiTerm>();
  settings = input<EditorSettings>();

  public id = computed(() => this.term().term);
  public controlId = computed(() => this.term().term + '-control');
  public controlHelpId = computed(() => this.term().term + '-control-help');
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

  private highlightService = inject(CiiFormHighlightTermService);
  protected isTermHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term().term);

  private remarkHighlightService = inject(CiiFormHighlightRemarkService);
  protected isRemarkHighlighted = computed(() => this.remarkHighlightService.highlightedRemark() === this.term().term);

  private chorusProRemarkHighlightService = inject(CiiFormHighlightChorusProRemarkService);
  protected isChorusProRemarkHighlighted = computed(() => this.chorusProRemarkHighlightService.highlightedChorusProRemark() === this.term().term);
}
