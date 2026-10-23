import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OperationResult } from '../../../../../core/models/common/api-response.model';
import {
  CreateOptionValuePayload,
  OptionValue,
  UpdateOptionValuePayload,
} from '../../../../models/catalog/category/option-value/option-value.model';
import { environment } from '../../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class OptionValueService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/sporthub/admin/option-value`;

  create(payload: CreateOptionValuePayload): Observable<OperationResult<OptionValue[]>> {
    return this.http.post<OperationResult<OptionValue[]>>(`${this.apiUrl}/create`, payload);
  }

  update(payload: UpdateOptionValuePayload): Observable<OperationResult<OptionValue>> {
    return this.http.post<OperationResult<OptionValue>>(`${this.apiUrl}/update`, payload);
  }

  delete(id: number): Observable<OperationResult<boolean>> {
    return this.http.post<OperationResult<boolean>>(`${this.apiUrl}/delete`, { id });
  }
}
