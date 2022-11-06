export interface Pagination {
  pageIndex: number;
  pageSize: number;
  itemCount: number;
}

export class DefaultPagination implements Pagination {
  pageIndex = 1;
  pageSize = 10;
  itemCount = 0;
}
