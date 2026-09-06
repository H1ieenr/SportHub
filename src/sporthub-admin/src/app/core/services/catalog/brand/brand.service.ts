import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OperationResult, PagedResult, PaginationParams } from '../../../../core/models/common/api-response.model';
import { Brand, BrandPayload } from '../../../models/catalog/brand/brand.model';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class BrandService {
    private http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/sporthub/admin/brand`;

    getList(params: PaginationParams): Observable<PagedResult<Brand>> {
        let httpParams = new HttpParams()
            .set('page_number', params.page_number.toString())
            .set('page_size', params.page_size.toString());

            if (params.sort_by) httpParams = httpParams.set('sort_by', params.sort_by);
            if (params.sort_dir) httpParams = httpParams.set('sort_dir', params.sort_dir);
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
        return this.http.post<OperationResult<Brand>>(`${this.apiUrl}/create`, payload);
    }

    update(id: number, payload: BrandPayload): Observable<OperationResult<Brand>> {
        return this.http.post<OperationResult<Brand>>(`${this.apiUrl}/update`, { id, ...payload });
    }

    toggleActive(id: number): Observable<OperationResult<null>> {
        return this.http.post<OperationResult<null>>(`${this.apiUrl}/active`, { id });
    }

    delete(id: number): Observable<OperationResult<boolean>> {
        return this.http.delete<OperationResult<boolean>>(`${this.apiUrl}/delete/${id}`);
    }
}