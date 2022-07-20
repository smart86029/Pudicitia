export interface Pagination {
  pageIndex: number;
  pageSize: number;
  itemCount: number;
}

export class DefaultPagination implements Pagination {
  pageIndex: number;
  pageSize: number;
  itemCount: number;

  constructor(
    pageIndex?: number,
    pageSize?: number,
    itemCount?: number,
  ) {
    this.pageIndex = pageIndex || 1;
    this.pageSize = pageSize || 10;
    this.itemCount = itemCount || 0;
  }
}
