import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dictionary',
})
export class DictionaryPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any[] {
    const result = [];
    if (this.isEnum(value)) {
      const keys = Object.keys(value);
      keys
        .slice(keys.length / 2)
        .forEach(key => result.push({ key, value: value[key] }));
    }
    return result;
  }

  private isEnum(input: object): boolean {
    const keys = Object.keys(input);
    const values = [];
    keys.forEach(key => values.push(input[key]));
    keys.forEach(key => {
      if (!values.includes(key)) {
        return false;
      }
    });
    return true;
  }
}
