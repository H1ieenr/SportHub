export interface OperationResult<T> {
    is_success: boolean;
    data: T;
    message: string;
    code: string;
}

export interface PagedResult<T> {
  results: T[];
  page_number: number;
  page_size: number;
  total_count: number;
  total_pages: number;
  has_previous_page: boolean;
  has_next_page: boolean;
}

export interface PaginationParams {
  page_number: number;
  page_size: number;
  sort_by?: string | null;
  sort_dir?: string;
  search_text?: string;
}