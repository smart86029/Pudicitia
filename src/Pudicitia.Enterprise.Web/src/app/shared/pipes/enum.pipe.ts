import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enum',
})
export class EnumPipe implements PipeTransform {
  transform<TValue>(value: TValue | null | undefined, input: { [key: string]: TValue }): string {
    if (value === null || value === undefined) {
      return '';
    }
    const map = new Map<TValue, string>();
    if (this.isEnum(input)) {
      const keys = Object.keys(input);
      keys.slice(keys.length / 2).forEach(key => map.set(input[key], key));
    }
    return map.get(value) || '';
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
