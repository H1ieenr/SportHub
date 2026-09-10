import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OperationResult, PagedResult, PaginationParams } from '../../../../core/models/common/api-response.model';
import { Brand, BrandListParams, BrandPayload } from '../../../models/catalog/brand/brand.model';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class BrandService {
    private http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/sporthub/admin/brand`;

    getList(params: BrandListParams): Observable<PagedResult<Brand>> {
        let httpParams = new HttpParams()
            .set('page_number', params.page_number.toString())
            .set('page_size', params.page_size.toString());

        if (params.sort_by) httpParams = httpParams.set('sort_by', params.sort_by);
        if (params.sort_dir) httpParams = httpParams.set('sort_dir', params.sort_dir);
        if (params.search_text) httpParams = httpParams.set('search_text', params.search_text);
        if (params.active !== undefined) httpParams = httpParams.set('active', params.active);

        return this.http
            .get<OperationResult<PagedResult<Brand>>>(`${this.apiUrl}/list`, { params: httpParams })
            .pipe(map((res) => res.data));
    }

    getView(id: number): Observable<Brand> {
        const httpParams = new HttpParams().set('id', id);
        return this.http
            .get<OperationResult<Brand>>(`${this.apiUrl}/view`, { params: httpParams })
            .pipe(map((res) => res.data));
    }

    create(payload: BrandPayload): Observable<OperationResult<Brand>> {
        const formData = this.buildFormData(payload);
        return this.http.post<OperationResult<Brand>>(`${this.apiUrl}/create`, formData);
    }

    update(id: number, payload: BrandPayload): Observable<OperationResult<Brand>> {
        const formData = this.buildFormData(payload);
        formData.append('id', String(id));
        return this.http.post<OperationResult<Brand>>(`${this.apiUrl}/update`, formData);
    }

    toggleActive(id: number): Observable<OperationResult<boolean>> {
        return this.http.post<OperationResult<boolean>>(`${this.apiUrl}/active`, { id });
    }

    delete(id: number): Observable<OperationResult<boolean>> {
        return this.http.post<OperationResult<boolean>>(`${this.apiUrl}/delete`, { id });
    }

    private buildFormData(payload: BrandPayload): FormData {
        const formData = new FormData();
        formData.append('name', payload.name);
        formData.append('slug', payload.slug);
        formData.append('description', payload.description ?? '');
        if (payload.is_active !== undefined) {
            formData.append('is_active', String(payload.is_active));
        }
        if (payload.file_logo) {
            formData.append('file_logo', payload.file_logo);
        }
        return formData;
    }
}