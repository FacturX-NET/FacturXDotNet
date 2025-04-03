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
  selector: 'app-cii-form-control',
  imports: [ReactiveFormsModule, NgTemplateOutlet, CiiFormRemarkComponent, CiiFormBusinessRulesComponent],
  template: `
    <div class="overflow-hidden px-1">
      <div class="editor__control">
        <label [id]="id()" class="form-label text-truncate" [class.text-primary]="isTermHighlighted()" [for]="controlId()">
          <span class="fw-semibold">{{ term() }}</span> - {{ name() }}
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
            <app-cii-form-business-rules [businessRules]="businessRules"></app-cii-form-business-rules>
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
    </div>
  `,
  styles: `
    .editor__control {
      max-width: 420px;
    }
  `,
})
export class CiiFormControlComponent {
  term = input.required<string>();
  name = input.required<string>();
  description = input<TemplateRef<any>>();
  businessRules = input<BusinessRuleTemplate[]>();
  remarks = input<TemplateRef<any>[]>();
  chorusProRemarks = input<TemplateRef<any>[]>();
  settings = input<EditorSettings>();

  public id = computed(() => this.term());
  public controlId = computed(() => this.term() + '-control');
  public controlHelpId = computed(() => this.term() + '-control-help');

  private highlightService = inject(CiiFormHighlightTermService);
  protected isTermHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term());

  private remarkHighlightService = inject(CiiFormHighlightRemarkService);
  protected isRemarkHighlighted = computed(() => this.remarkHighlightService.highlightedRemark() === this.term());

  private chorusProRemarkHighlightService = inject(CiiFormHighlightChorusProRemarkService);
  protected isChorusProRemarkHighlighted = computed(() => this.chorusProRemarkHighlightService.highlightedChorusProRemark() === this.term());
}
