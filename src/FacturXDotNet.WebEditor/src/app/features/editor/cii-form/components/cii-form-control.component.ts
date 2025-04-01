import { Component, computed, inject, input, signal, TemplateRef } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorSettings } from '../../editor-settings.service';
import { NgTemplateOutlet } from '@angular/common';
import { BusinessRuleTemplate } from './business-rule-template';
import { CiiFormRemarkComponent } from './cii-form-remark.component';
import { CiiFormHighlightService } from '../services/cii-form-highlight.service';

@Component({
  selector: 'app-cii-form-control',
  imports: [ReactiveFormsModule, NgTemplateOutlet, CiiFormRemarkComponent],
  template: `
    <div class="overflow-hidden" (mouseenter)="highlight(true)" (mouseleave)="highlight(false)">
      <div class="editor__control">
        <label [id]="id()" class="form-label text-truncate" [class.text-primary]="isHighlighted()" [for]="controlId()">
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

  private highlightService = inject(CiiFormHighlightService);
  protected isHighlighted = computed(() => this.highlightService.highlightedTerm() === this.term());

  protected highlight(value: boolean) {
    this.highlightService.highlightTerm(this.term(), value);
  }
}
