import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightService {
  private highlightedTermInternal = signal<string | undefined>(undefined);
  public highlightedTerm = this.highlightedTermInternal.asReadonly();

  private highlighted: string[] = [];

  /**
   * Highlight the given term.
   * There is always at most one term highlighted. When a new term is highlighted, the previous one is unhighlighted.
   * When a term is unhighlighted, the last highlighted term is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightTerm(id: string, highlight: boolean = true) {
    if (highlight) {
      this.highlighted.push(id);
      this.highlightedTermInternal.set(id);
    } else {
      this.highlighted = this.highlighted.filter((term) => term !== id);
      if (this.highlighted.length == 0) {
        this.highlightedTermInternal.set(undefined);
      } else {
        this.highlightedTermInternal.set(this.highlighted[this.highlighted.length - 1]);
      }
    }
  }
}
