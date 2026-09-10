import { PaginationParams } from "../../common/api-response.model";

export interface Category {
  id: number;
  name: string;
  slug: string;
  description: string;
  image_url: string | null;
  image_public_id: string | null;
  parent_id: number | null;
  display_order: number;
  is_active: boolean;
  created_date: string;
  created_by: number;
  updated_date: string | null;
  updated_by: number;
}

export interface CategoryPayload {
  name: string;
  slug: string;
  description: string;
  parent_id: number | null;
  display_order: number;
  is_active?: boolean;
  file_image?: File | null;
}

export interface CategoryNode extends Category {
  level: number;
  has_children: boolean;
  expanded: boolean;
  parent_name?: string | null;
  groupIndex: number;
}

export interface CategoryListParams extends PaginationParams {
  parent_id?: number | null;
  active?: boolean;
}
export interface CategoryListNoPagingParams {
  parent_id?: number | null;
  active?: boolean;
  search_text?: string;
}