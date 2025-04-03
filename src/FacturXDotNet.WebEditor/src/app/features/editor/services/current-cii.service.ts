import { Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { CrossIndustryInvoice } from '../../../core/facturx-models/cii/cross-industry-invoice';

@Injectable({
  providedIn: 'root',
})
export class CurrentCiiService {
  get current() {
    return this.currentInternal.asReadonly();
  }
  private currentInternal: WritableSignal<CrossIndustryInvoice>;

  private localStorageKey = 'saved-cii';

  constructor() {
    const cii = this.loadCii();
    this.currentInternal = signal(cii ?? {});
  }

  update(value: CrossIndustryInvoice): void {
    this.saveCii(value);
    this.currentInternal.set(value);
  }

  clear() {
    this.clearCii();
  }

  private saveCii(value: CrossIndustryInvoice): void {
    const ciiStr = JSON.stringify(value);
    localStorage.setItem(this.localStorageKey, ciiStr);
  }

  private loadCii(): CrossIndustryInvoice | null {
    const ciiStr = localStorage.getItem(this.localStorageKey);
    if (ciiStr === null) {
      return null;
    }

    return JSON.parse(ciiStr);
  }

  private clearCii() {
    localStorage.removeItem(this.localStorageKey);
  }
}
