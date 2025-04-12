import { Component, computed, ElementRef, inject, input, numberAttribute, TemplateRef, viewChild } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgTemplateOutlet } from '@angular/common';
import { CiiFormRemarkComponent } from './cii-form-remark.component';
import { BusinessRuleTemplate, CiiFormBusinessRulesComponent } from './cii-form-business-rules.component';
import { EditorSettings } from '../../../../editor-settings.service';
import { CiiFormHighlightTermService } from '../../cii-form-highlight-term.service';
import { CiiFormHighlightRemarkService } from '../../cii-form-highlight-remark.service';
import { CiiFormHighlightChorusProRemarkService } from '../../cii-form-highlight-chorus-pro-remark.service';
import { CiiTerm } from '../../constants/cii-terms';

@Component({
  selector: 'app-cii-form-parent-container',
  imports: [ReactiveFormsModule, NgTemplateOutlet, CiiFormRemarkComponent, CiiFormBusinessRulesComponent],
  template: `
    <h6 #title [id]="term()" class="py-2 m-0 sticky-top bg-body" style="top: {{ topPx() }}px; z-index: {{ zIndex() }}" [class.text-primary]="isTermHighlighted()">
      {{ term().term }} - {{ term().name }}
    </h6>

    <div class="ps-3 border-start border-2" [class.border-primary]="isTermHighlighted()">
      <div class="form-text" [class.text-primary]="isTermHighlighted()">
        @if (description(); as description) {
          <ng-container [ngTemplateOutlet]="description"></ng-container>
        }
      </div>

      <div class="pb-2"><!--spacer--></div>

      @if (settings(); as settings) {
        @if (settings?.showBusinessRules === true) {
          @if (businessRules(); as businessRules) {
            <div [id]="businessRulesId()">
              <app-cii-form-business-rules [businessRules]="businessRules" [highlight]="isTermHighlighted()"></app-cii-form-business-rules>
            </div>
          }
        }

        @if (remarks(); as remarks) {
          @if (remarks.length > 0 && settings.showRemarks) {
            <div [id]="remarkId()">
              @for (remark of remarks; track remark) {
                <app-cii-form-remark [highlight]="isTermHighlighted() || isRemarkHighlighted()">
                  <ng-container [ngTemplateOutlet]="remark"></ng-container>
                </app-cii-form-remark>
              }
            </div>
          }
        }

        @if (chorusProRemarks(); as chorusProRemarks) {
          @if (chorusProRemarks.length > 0 && settings.showChorusProRemarks) {
            <div [id]="chorusProRemarkId()">
              @for (chorusProRemark of chorusProRemarks; track chorusProRemark) {
                <app-cii-form-remark title="CHORUSPRO" [highlight]="isTermHighlighted() || isChorusProRemarkHighlighted()">
                  <ng-container [ngTemplateOutlet]="chorusProRemark"></ng-container>
                </app-cii-form-remark>
              }
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
  description = input<TemplateRef<any>>();
  businessRules = input<BusinessRuleTemplate[]>();
  remarks = input<TemplateRef<any>[]>();
  chorusProRemarks = input<TemplateRef<any>[]>();
  settings = input<EditorSettings>();
  depth = input.required({ transform: numberAttribute });

  public businessRulesId = computed(() => this.term().term + '-rules');
  public remarkId = computed(() => this.term().term + '-remarks');
  public chorusProRemarkId = computed(() => this.term().term + '-cpro-remarks');

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
