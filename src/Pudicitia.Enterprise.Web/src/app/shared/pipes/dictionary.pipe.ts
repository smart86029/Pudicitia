import { KeyValue } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dictionary',
})
export class DictionaryPipe implements PipeTransform {
  transform<TValue>(input: { [key: string]: TValue }): KeyValue<string, TValue>[] {
    const result: KeyValue<string, TValue>[] = [];
    if (this.isEnum(input)) {
      const keys = Object.keys(input);
      keys.slice(keys.length / 2).forEach(key => result.push({ key, value: input[key] }));
    }
    return result;
  }

  private isEnum<TValue>(input: { [key: string]: TValue }): boolean {
    const keys = Object.keys(input);
    const values: string[] = [];
    let result = true;
    keys.forEach(key => values.push(String(input[key])));
    keys.forEach(key => {
      if (!values.includes(key)) {
        result = false;
      }
    });
    return result;
  }
}
