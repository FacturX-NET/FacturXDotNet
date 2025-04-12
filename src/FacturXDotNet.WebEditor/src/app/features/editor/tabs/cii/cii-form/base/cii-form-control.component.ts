import { Component, computed, inject, input, TemplateRef } from '@angular/core';
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
  selector: 'app-cii-form-control',
  imports: [ReactiveFormsModule, NgTemplateOutlet, CiiFormRemarkComponent, CiiFormBusinessRulesComponent],
  template: `
    <div class="overflow-hidden px-1">
      <div class="editor__control">
        <label [id]="id()" class="form-label text-truncate" [class.text-primary]="isTermHighlighted()" [for]="controlId()">
          <span class="fw-semibold">{{ term().term }}</span> - {{ term().name }}
        </label>

        <ng-content></ng-content>
      </div>

      @if (description(); as description) {
        <p [id]="controlHelpId()" class="form-text">
          <ng-container [ngTemplateOutlet]="description"></ng-container>
        </p>
      } @else {
        <div class="pb-3"><!--spacer--></div>
      }

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
    </div>
  `,
})
export class CiiFormControlComponent {
  term = input.required<CiiTerm>();
  description = input<TemplateRef<any>>();
  businessRules = input<BusinessRuleTemplate[]>();
  remarks = input<TemplateRef<any>[]>();
  chorusProRemarks = input<TemplateRef<any>[]>();
  settings = input<EditorSettings>();

  public id = computed(() => this.term().term);
  public controlId = computed(() => this.term().term + '-control');
  public controlHelpId = computed(() => this.term().term + '-control-help');
  public businessRulesId = computed(() => this.term().term + '-rules');
  public remarkId = computed(() => this.term().term + '-remarks');
  public chorusProRemarkId = computed(() => this.term().term + '-cpro-remarks');

  private highlightService = inject(CiiFormHighlightTermService);
  protected isTermHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term().term);

  private remarkHighlightService = inject(CiiFormHighlightRemarkService);
  protected isRemarkHighlighted = computed(() => this.remarkHighlightService.highlightedRemark() === this.term().term);

  private chorusProRemarkHighlightService = inject(CiiFormHighlightChorusProRemarkService);
  protected isChorusProRemarkHighlighted = computed(() => this.chorusProRemarkHighlightService.highlightedChorusProRemark() === this.term().term);
}
