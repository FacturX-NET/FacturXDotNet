import { Injectable } from '@angular/core';
import { debounceTime, Subject } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CiiFormHighlightChorusProRemarkService {
  private highlightedChorusProRemarkSubject = new Subject<string | undefined>();
  public highlightedChorusProRemark = toSignal(this.highlightedChorusProRemarkSubject.pipe(debounceTime(50)));

  private highlightedChorusProRemarks: string[] = [];

  /**
   * Highlight the given chorus pro remark.
   * There is always at most one chorus pro remark highlighted. When a new chorus pro remark is highlighted, the previous one is unhighlighted.
   * When a chorus pro remark is unhighlighted, the last highlighted chorus pro remark is highlighted again (unless it has been unhighlighted in the meantime).
   */
  highlightChorusProRemark(id: string, highlight: boolean = true) {
    if (highlight) {
      this.highlightedChorusProRemarks.push(id);
      this.highlightedChorusProRemarkSubject.next(id);
    } else {
      this.highlightedChorusProRemarks = this.highlightedChorusProRemarks.filter((chorusProRemark) => chorusProRemark !== id);
      if (this.highlightedChorusProRemarks.length == 0) {
        this.highlightedChorusProRemarkSubject.next(undefined);
      } else {
        this.highlightedChorusProRemarkSubject.next(this.highlightedChorusProRemarks[this.highlightedChorusProRemarks.length - 1]);
      }
    }
  }
}
