import { Component, computed, contentChildren, effect, inject, input, signal, TemplateRef, viewChildren } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../editor-settings.service';
import { NgTemplateOutlet } from '@angular/common';
import { BusinessRuleTemplate } from './business-rule-template';
import { CiiFormRemarkComponent } from './cii-form-remark.component';
import { CiiFormControlComponent } from './cii-form-control.component';
import { CiiFormHighlightService } from '../services/cii-form-highlight.service';

@Component({
  selector: 'app-cii-form-parent-container',
  imports: [ReactiveFormsModule, NgTemplateOutlet, CiiFormRemarkComponent],
  template: `
    <h6 [id]="term()" class="m-0" [class.text-primary]="isHighlighted()">{{ term() }} - {{ name() }}</h6>

    <div class="ps-3 border-start border-2" [class.border-primary]="isHighlighted()" (mouseenter)="highlight(true)" (mouseleave)="highlight(false)">
      <div class="form-text">
        @if (description(); as description) {
          <ng-container [ngTemplateOutlet]="description"></ng-container>
        }
      </div>

      <div class="pb-2"><!--spacer--></div>

      @if (settings(); as settings) {
        @if (settings?.showBusinessRules === true) {
          @if (businessRules(); as businessRules) {
            <div class="form-text">
              <div class="fw-semibold">Business Rules</div>
              <ul>
                @for (rule of businessRules; track rule.id) {
                  <li>
                    <span [id]="rule.id" class="fw-semibold">{{ rule.id }}</span
                    >:
                    <ng-container [ngTemplateOutlet]="rule.template"></ng-container>
                  </li>
                }
              </ul>
            </div>
          }
        }

        @if (remarks(); as remarks) {
          @if (remarks.length > 0 && settings.showRemarks) {
            <div [id]="term() + '-remarks'">
              @for (remark of remarks; track remark) {
                <app-cii-form-remark [highlight]="isHighlighted()">
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
                <app-cii-form-remark title="CHORUSPRO" [highlight]="isHighlighted()">
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

  private highlightService = inject(CiiFormHighlightService);
  protected isHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term());

  protected highlight(value: boolean) {
    this.highlightService.highlightTerm(this.term(), value);
  }
}
