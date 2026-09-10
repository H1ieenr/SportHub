import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OperationResult, PagedResult, PaginationParams } from '../../../../core/models/common/api-response.model';
import { Category, CategoryListNoPagingParams, CategoryListParams, CategoryPayload } from '../../../models/catalog/category/category.model';
import { environment } from '../../../../../environments/environment';



@Injectable({ providedIn: 'root' })
export class CategoryService {
    private http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/sporthub/admin/category`;

    getList(params: CategoryListParams): Observable<PagedResult<Category>> {
        let httpParams = new HttpParams()
            .set('page_number', params.page_number.toString())
            .set('page_size', params.page_size.toString());

        if (params.sort_by) httpParams = httpParams.set('sort_by', params.sort_by);
        if (params.sort_dir) httpParams = httpParams.set('sort_dir', params.sort_dir);
        if (params.search_text) httpParams = httpParams.set('search_text', params.search_text);
        if (params.active !== undefined) httpParams = httpParams.set('active', params.active);
        if (params.parent_id !== undefined) {
            httpParams = httpParams.set('parent_id', params.parent_id === null ? '' : params.parent_id.toString());
        }

        return this.http
            .get<OperationResult<PagedResult<Category>>>(`${this.apiUrl}/list`, { params: httpParams })
            .pipe(map((res) => res.data));
    }

    getAll(params: CategoryListNoPagingParams): Observable<Category[]> {
        let httpParams = new HttpParams();
        if (params.search_text) httpParams = httpParams.set('search_text', params.search_text);
        if (params.active !== undefined) httpParams = httpParams.set('active', params.active);
        if (params.parent_id !== undefined) {
            httpParams = httpParams.set('parent_id', params.parent_id === null ? '' : params.parent_id.toString());
        }
        return this.http
            .get<OperationResult<Category[]>>(`${this.apiUrl}/list-nopaging`, { params: httpParams })
            .pipe(map((res) => res.data));
    }

    getView(id: number): Observable<Category> {
        const httpParams = new HttpParams().set('id', id);
        return this.http
            .get<OperationResult<Category>>(`${this.apiUrl}/view`, { params: httpParams })
            .pipe(map((res) => res.data));
    }

    create(payload: CategoryPayload): Observable<OperationResult<Category>> {
        const formData = this.buildFormData(payload);
        return this.http.post<OperationResult<Category>>(`${this.apiUrl}/create`, formData);
    }

    update(id: number, payload: CategoryPayload): Observable<OperationResult<Category>> {
        const formData = this.buildFormData(payload);
        formData.append('id', String(id));
        return this.http.post<OperationResult<Category>>(`${this.apiUrl}/update`, formData);
    }

    toggleActive(id: number): Observable<OperationResult<boolean>> {
        return this.http.post<OperationResult<boolean>>(`${this.apiUrl}/active`, { id });
    }

    delete(id: number): Observable<OperationResult<boolean>> {
        return this.http.post<OperationResult<boolean>>(`${this.apiUrl}/delete`, { id });
    }

    private buildFormData(payload: CategoryPayload): FormData {
        const formData = new FormData();
        formData.append('name', payload.name);
        formData.append('slug', payload.slug);
        formData.append('description', payload.description ?? '');
        formData.append('display_order', String(payload.display_order ?? 0));
        if (payload.parent_id !== null && payload.parent_id !== undefined) {
            formData.append('parent_id', String(payload.parent_id));
        }
        if (payload.is_active !== undefined) {
            formData.append('is_active', String(payload.is_active));
        }
        if (payload.file_image) {
            formData.append('file_image', payload.file_image);
        }
        return formData;
    }
}