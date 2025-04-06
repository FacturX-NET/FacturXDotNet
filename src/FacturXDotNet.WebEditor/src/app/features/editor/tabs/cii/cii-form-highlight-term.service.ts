import { Injectable } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightTermService {
  private highlightedTermSubject = new Subject<string | undefined>();
  public highlightedTerm = toSignal(this.highlightedTermSubject.pipe(debounceTime(50)));

  private highlightedTerms: string[] = [];

  /**
   * Highlight the given term.
   * There is always at most one term highlighted. When a new term is highlighted, the previous one is unhighlighted.
   * When a term is unhighlighted, the last highlighted term is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightTerm(id: string, highlight: boolean = true) {
    if (highlight) {
      this.highlightedTerms.push(id);
      this.highlightedTermSubject.next(id);
    } else {
      this.highlightedTerms = this.highlightedTerms.filter((term) => term !== id);
      if (this.highlightedTerms.length == 0) {
        this.highlightedTermSubject.next(undefined);
      } else {
        this.highlightedTermSubject.next(this.highlightedTerms[this.highlightedTerms.length - 1]);
      }
    }
  }
}
