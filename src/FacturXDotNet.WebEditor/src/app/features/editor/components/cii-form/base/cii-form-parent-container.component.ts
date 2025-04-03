import { Component, computed, inject, input, TemplateRef } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../../editor-settings.service';
import { NgTemplateOutlet } from '@angular/common';
import { CiiFormRemarkComponent } from './cii-form-remark.component';
import { CiiFormHighlightTermService } from '../../../services/cii-form-highlight-term.service';
import { BusinessRuleTemplate, CiiFormBusinessRulesComponent } from './cii-form-business-rules.component';
import { CiiFormHighlightRemarkService } from '../../../services/cii-form-highlight-remark.service';
import { CiiFormHighlightChorusProRemarkService } from '../../../services/cii-form-highlight-chorus-pro-remark.service';

@Component({
  selector: 'app-cii-form-parent-container',
  imports: [ReactiveFormsModule, NgTemplateOutlet, CiiFormRemarkComponent, CiiFormBusinessRulesComponent],
  template: `
    <h6 [id]="term()" class="my-2 mx-0" [class.text-primary]="isTermHighlighted()">{{ term() }} - {{ name() }}</h6>

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
            <div [id]="term() + '-rules'">
              <app-cii-form-business-rules [businessRules]="businessRules" [highlight]="isTermHighlighted()"></app-cii-form-business-rules>
            </div>
          }
        }

        @if (remarks(); as remarks) {
          @if (remarks.length > 0 && settings.showRemarks) {
            <div [id]="term() + '-remarks'">
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
            <div [id]="term() + '-cpro-remarks'">
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
  term = input.required<string>();
  name = input.required<string>();
  description = input<TemplateRef<any>>();
  businessRules = input<BusinessRuleTemplate[]>();
  remarks = input<TemplateRef<any>[]>();
  chorusProRemarks = input<TemplateRef<any>[]>();
  settings = input<EditorSettings>();

  private highlightService = inject(CiiFormHighlightTermService);
  protected isTermHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term());

  private remarkHighlightService = inject(CiiFormHighlightRemarkService);
  protected isRemarkHighlighted = computed(() => this.remarkHighlightService.highlightedRemark() === this.term());

  private chorusProRemarkHighlightService = inject(CiiFormHighlightChorusProRemarkService);
  protected isChorusProRemarkHighlighted = computed(() => this.chorusProRemarkHighlightService.highlightedChorusProRemark() === this.term());
}
