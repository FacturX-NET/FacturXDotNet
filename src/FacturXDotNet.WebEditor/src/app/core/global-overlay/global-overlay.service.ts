import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GlobalOverlayService {
  public enabled = signal(false);
  public message = signal<string | undefined>(undefined);

  public enable(message?: string) {
    this.enabled.set(true);
    this.message.set(message);

    console.log('Overlay enabled:', message);
  }

  public disable() {
    this.enabled.set(false);

    console.log('Overlay disabled');
  }
}
