export interface FlatNode<TValue> {
  expandable: boolean;
  name: string;
  value: TValue;
  level: number;
}
