import { PagedResult } from './api-response.model';

export interface ListPageState<T> {
  loading: boolean;
  errorMessage: string | null;
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  searchText: string;
}

export function initialListPageState<T>(pageSize = 50): ListPageState<T> {
  return {
    loading: false,
    errorMessage: null,
    items: [],
    pageNumber: 1,
    pageSize,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false,
    hasNextPage: false,
    searchText: ''
  };
}

export function applyPagedResult<T>(
  state: ListPageState<T>,
  result: PagedResult<T>
): ListPageState<T> {
  return {
    ...state,
    items: result.results,
    pageNumber: result.page_number,
    pageSize: result.page_size,
    totalCount: result.total_count,
    totalPages: result.total_pages,
    hasPreviousPage: result.has_previous_page,
    hasNextPage: result.has_next_page
  };
}