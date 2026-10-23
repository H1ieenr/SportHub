import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OperationResult } from '../../../../../core/models/common/api-response.model';
import {
  CreateOptionDefinitionPayload,
  OptionDefinition,
  OptionDefinitionQuery,
  UpdateOptionDefinitionPayload,
} from '../../../../models/catalog/category/option-definition/option-definition.model';
import { environment } from '../../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class OptionDefinitionService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/sporthub/admin/option-definition`;

  getList(params: OptionDefinitionQuery = {}): Observable<OptionDefinition[]> {
    let httpParams = new HttpParams();

    if (params.search_text) httpParams = httpParams.set('search_text', params.search_text);
    if (params.code) httpParams = httpParams.set('code', params.code);
    for (const id of params.ids ?? []) {
      httpParams = httpParams.append('ids', String(id));
    }

    return this.http
      .get<OperationResult<OptionDefinition[]>>(`${this.apiUrl}/list-nopaging`, {
        params: httpParams,
      })
      .pipe(map((response) => response.data));
  }

  create(payload: CreateOptionDefinitionPayload): Observable<OperationResult<OptionDefinition>> {
    return this.http.post<OperationResult<OptionDefinition>>(`${this.apiUrl}/create`, payload);
  }

  update(payload: UpdateOptionDefinitionPayload): Observable<OperationResult<OptionDefinition>> {
    return this.http.post<OperationResult<OptionDefinition>>(`${this.apiUrl}/update`, payload);
  }

  delete(id: number): Observable<OperationResult<boolean>> {
    return this.http.post<OperationResult<boolean>>(`${this.apiUrl}/delete`, { id });
  }
}
