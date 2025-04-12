import { Injectable } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { BusinessRuleIdentifier } from './constants/cii-business-rules';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightBusinessRuleService {
  private highlightedBusinessRuleSubject = new Subject<BusinessRuleIdentifier | undefined>();
  public highlightedBusinessRule = toSignal(this.highlightedBusinessRuleSubject.pipe(debounceTime(50)));

  private highlightedBusinessRules: BusinessRuleIdentifier[] = [];

  /**
   * Highlight the given business rule.
   * There is always at most one business rule highlighted. When a new business rule is highlighted, the previous one is unhighlighted.
   * When a business rule is unhighlighted, the last highlighted business rule is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightBusinessRule(id: BusinessRuleIdentifier, highlight: boolean = true) {
    if (highlight) {
      this.highlightedBusinessRules.push(id);
      this.highlightedBusinessRuleSubject.next(id);
    } else {
      this.highlightedBusinessRules = this.highlightedBusinessRules.filter((br) => br !== id);
      if (this.highlightedBusinessRules.length == 0) {
        this.highlightedBusinessRuleSubject.next(undefined);
      } else {
        this.highlightedBusinessRuleSubject.next(this.highlightedBusinessRules[this.highlightedBusinessRules.length - 1]);
      }
    }
  }
}
