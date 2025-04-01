import { Component, input, signal, TemplateRef } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EditorSettings, EditorSettingsService } from '../../editor-settings.service';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-cii-form-parent-container',
  imports: [ReactiveFormsModule, NgTemplateOutlet],
  template: `
    <h6 [id]="term()" class="m-0" [class.text-primary]="isContentHighlighted()">{{ term() }} - {{ name() }}</h6>

    <div class="ps-3 border-start border-2" [class.border-primary]="isContentHighlighted()" (mouseenter)="highlightContent(true)" (mouseleave)="highlightContent(false)">
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
            <div [id]="term() + '-remark'">
              @for (remark of remarks; track remark) {
                <div class="alert alert-light small">
                  <i class="bi bi-info-circle pe-1"></i>
                  <ng-container [ngTemplateOutlet]="remark"></ng-container>
                </div>
              }
            </div>
          }
        }

        @if (chorusProRemarks(); as chorusProRemarks) {
          @if (chorusProRemarks.length > 0 && settings.showChorusProRemarks) {
            <div [id]="term() + '-cpro-remark'">
              @for (chorusProRemark of chorusProRemarks; track chorusProRemark) {
                <div class="alert alert-light small">
                  <div class="fw-semibold"><i class="bi bi-info-circle"></i> CHORUSPRO</div>
                  <ng-container [ngTemplateOutlet]="chorusProRemark"></ng-container>
                </div>
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

  protected isContentHighlighted = signal<boolean>(false);

  protected highlightContent(highlight: boolean) {
    this.isContentHighlighted.set(highlight);
  }
}

export interface BusinessRuleTemplate {
  readonly id: string;
  readonly template: TemplateRef<any>;
}
