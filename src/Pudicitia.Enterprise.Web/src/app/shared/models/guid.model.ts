export class Guid {
  private static validator = new RegExp('^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$', 'i');
  static empty = new Guid('00000000-0000-0000-0000-000000000000');

  private value: string;

  constructor(value: string) {
    if (!value) {
      throw new TypeError('Invalid argument; `value` has no value.');
    }
    this.value = Guid.isGuid(value) ? value : '00000000-0000-0000-0000-000000000000';
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  static isGuid(value: any): boolean {
    return !!value && (value instanceof Guid || Guid.validator.test(value.toString()));
  }

  static parse(value: string | null): Guid {
    return this.isGuid(value) ? new Guid(value!) : this.empty;
  }

  equals(other: Guid): boolean {
    return Guid.isGuid(other) && this.value === other.toString();
  }

  toString(): string {
    return this.value;
  }
}
