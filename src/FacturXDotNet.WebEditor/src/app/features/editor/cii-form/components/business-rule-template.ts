import { TemplateRef } from '@angular/core';

export interface BusinessRuleTemplate {
  readonly id: string;
  readonly template: TemplateRef<any>;
}
