export interface PaginationOutput<TItem> {
  pageIndex: number;
  pageSize: number;
  itemCount: number;
  items: TItem[];
}

export class DefaultPaginationOutput<TItem> implements PaginationOutput<TItem> {
  pageIndex: number;
  pageSize: number;
  itemCount: number;
  items: TItem[] = [];

  constructor(
    pageIndex?: number,
    pageSize?: number,
    itemCount?: number,
    items?: TItem[],
  ) {
    this.pageIndex = pageIndex || 1;
    this.pageSize = pageSize || 10;
    this.itemCount = itemCount || 0;
    this.items = items || [];
  }
}
