import { Injectable } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightRemarkService {
  private highlightedRemarkSubject = new Subject<string | undefined>();
  public highlightedRemark = toSignal(this.highlightedRemarkSubject.pipe(debounceTime(50)));

  private highlightedRemarks: string[] = [];

  /**
   * Highlight the given remark.
   * There is always at most one remark highlighted. When a new remark is highlighted, the previous one is unhighlighted.
   * When a remark is unhighlighted, the last highlighted remark is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightRemark(id: string, highlight: boolean = true) {
    if (highlight) {
      this.highlightedRemarks.push(id);
      this.highlightedRemarkSubject.next(id);
    } else {
      this.highlightedRemarks = this.highlightedRemarks.filter((remark) => remark !== id);
      if (this.highlightedRemarks.length == 0) {
        this.highlightedRemarkSubject.next(undefined);
      } else {
        this.highlightedRemarkSubject.next(this.highlightedRemarks[this.highlightedRemarks.length - 1]);
      }
    }
  }
}
