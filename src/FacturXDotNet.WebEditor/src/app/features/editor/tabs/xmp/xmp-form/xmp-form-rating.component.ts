import { Component, computed, forwardRef, input, numberAttribute, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-xmp-form-rating',
  imports: [],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => XmpFormRatingComponent),
    },
  ],
  template: `
    <div class="d-flex gap-1">
      <a role="button" (click)="set(-1)" (mouseenter)="preview(-1)" (mouseleave)="cancelPreview()">
        @if (rate() < 0) {
          <i class="bi bi-x-circle-fill text-danger"></i>
        } @else {
          <i class="bi bi-x-circle text-body-tertiary"></i>
        }
      </a>
      <a role="button" (click)="set(0)" (mouseenter)="preview(0)" (mouseleave)="cancelPreview()">
        <i class="bi bi-dot" [class.text-body-tertiary]="rate() != 0" [class.text-body]="rate() == 0"></i>
      </a>

      @for (value of values(); track value) {
        <a role="button" (click)="set(value)" (mouseenter)="preview(value)" (mouseleave)="cancelPreview()">
          @if (rate() >= value) {
            <i class="bi bi-star-fill star-color"></i>
          } @else {
            <i class="bi bi-star text-body-tertiary"></i>
          }
        </a>
      }

      <div class="ps-2">
        @switch (rate()) {
          @case (-1) {
            <span class="text-danger small">Rejected</span>
          }
          @case (0) {
            <span class="text-body-secondary small">Unrated</span>
          }
          @default {
            {{ rate() }} / {{ max() }}
          }
        }
      </div>
    </div>
  `,
  styles: `
    .star-color {
      color: rgba(255, 193, 7, 1);
    }
  `,
})
export class XmpFormRatingComponent implements ControlValueAccessor {
  max = input(5, { transform: numberAttribute });

  protected disabled = signal(false);
  protected value = signal(0);
  protected previewValue = signal<number | undefined>(undefined);

  protected rate = computed(() => {
    const preview = this.previewValue();
    const value = this.value();

    if (preview === undefined) {
      return value;
    }

    return preview;
  });

  protected values = computed(() => Array.from({ length: this.max() }, (_, i) => i + 1));

  private onChange: ((value: number) => void) | undefined;

  writeValue(value: number): void {
    this.value.set(this.sanitize(value));
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }

  registerOnTouched(fn: any): void {}

  protected set(value: number) {
    const sanitized = this.sanitize(value);

    this.value.set(sanitized);

    if (this.onChange === undefined) {
      return;
    }

    this.onChange(sanitized);
  }

  protected preview(value: number) {
    this.previewValue.set(value);
  }

  protected cancelPreview() {
    this.previewValue.set(undefined);
  }

  private sanitize(value: number) {
    const intValue = Math.floor(value);

    if (intValue < -1) {
      return -1;
    }

    if (intValue > this.max()) {
      return this.max();
    }

    return intValue;
  }
}
