import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'escapeHtml',
})
export class EscapeHtmlPipe implements PipeTransform {
  transform(value: string): string {
    return value.replaceAll('<', '&lt;').replaceAll('>', '&gt;');
  }
}
