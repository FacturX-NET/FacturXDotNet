import { computed, Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditorResponsivenessService {
  get leftColumnWidth() {
    return this.leftColumnWidthInternal.asReadonly();
  }

  private leftColumnWidthInternal = signal(0);

  smallLeftColumn = computed(() => {
    const width = this.leftColumnWidth();
    return width !== undefined && width < 950;
  });

  foldLeftColumn = computed(() => {
    const width = this.leftColumnWidth();
    return width !== undefined && width < 650;
  });

  setLeftColumnWidth(width: number) {
    this.leftColumnWidthInternal.set(width);
  }
}
