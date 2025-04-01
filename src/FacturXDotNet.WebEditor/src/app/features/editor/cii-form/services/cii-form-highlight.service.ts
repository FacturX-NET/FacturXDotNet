import { Injectable, signal } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightService {
  private highlightedTermSubject = new Subject();
  public highlightedTerm = toSignal(this.highlightedTermSubject.pipe(debounceTime(50)));

  private highlighted: string[] = [];

  /**
   * Highlight the given term.
   * There is always at most one term highlighted. When a new term is highlighted, the previous one is unhighlighted.
   * When a term is unhighlighted, the last highlighted term is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightTerm(id: string, highlight: boolean = true) {
    if (highlight) {
      this.highlighted.push(id);
      this.highlightedTermSubject.next(id);
    } else {
      this.highlighted = this.highlighted.filter((term) => term !== id);
      if (this.highlighted.length == 0) {
        this.highlightedTermSubject.next(undefined);
      } else {
        this.highlightedTermSubject.next(this.highlighted[this.highlighted.length - 1]);
      }
    }
  }
}
