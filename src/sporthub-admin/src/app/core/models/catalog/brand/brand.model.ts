import { PaginationParams } from "../../common/api-response.model";

export interface Brand {
  id: number;
  name: string;
  slug: string;
  logo_url: string;
  description: string;
  is_active: boolean;
  created_date: string;
  created_by: number;
  updated_date: string;
  updated_by: number;
}

export interface BrandPayload {
  name: string;
  slug: string;
  description: string;
  file_logo?: File | null;
  is_active?: boolean | undefined;
}

export interface BrandListParams extends PaginationParams {
  active?: boolean;
}