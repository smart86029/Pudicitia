export interface FlatNode<T> {
  expandable: boolean;
  name: string;
  value: T;
  level: number;
}
