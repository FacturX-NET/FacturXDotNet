import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'highlightText',
})
export class HighlightTextPipe implements PipeTransform {
  transform(value: string, args: string[] | undefined): any {
    if (args === undefined || args.length === 0) {
      return value;
    }

    let result = value;
    for (const arg of args) {
      const regex = new RegExp(arg.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'ig');
      result = result.replace(regex, `<span class='highlight'>$&</span>`);
    }
    return result;
  }
}
