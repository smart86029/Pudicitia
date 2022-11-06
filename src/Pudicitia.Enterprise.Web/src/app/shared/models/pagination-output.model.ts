import { DefaultPagination, Pagination } from './pagination.model';

export interface PaginationOutput<TItem> {
  page: Pagination;
  items: TItem[];
}

export class DefaultPaginationOutput<TItem> implements PaginationOutput<TItem> {
  page: Pagination = new DefaultPagination();
  items: TItem[] = [];
}
