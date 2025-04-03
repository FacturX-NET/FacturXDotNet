import { Injectable } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightBusinessRuleService {
  private highlightedBusinessRuleSubject = new Subject<string | undefined>();
  public highlightedBusinessRule = toSignal(this.highlightedBusinessRuleSubject.pipe(debounceTime(50)));

  private highlightedBusinessRules: string[] = [];

  /**
   * Highlight the given business rule.
   * There is always at most one business rule highlighted. When a new business rule is highlighted, the previous one is unhighlighted.
   * When a business rule is unhighlighted, the last highlighted business rule is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightBusinessRule(id: string, highlight: boolean = true) {
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
