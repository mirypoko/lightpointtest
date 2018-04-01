import { Injectable } from '@angular/core';
import { IProduct } from '../models/product';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ProductsService {

  protected apiUrl = 'api/products';

  constructor(protected _httpClient: HttpClient) { }

  public search(count: number, searchString: string, offset: number = 0, shopId = 0): Observable<Array<IProduct>> {
    let params = new HttpParams();
    if (offset > 0) {
      params = params.append('offset', offset.toString());
    }
    if (count > 0) {
      params = params.append('count', count.toString());
    }
    if (shopId > 0) {
      params = params.append('shopId', shopId.toString());
    }
    if (searchString.length > 2) {
      params = params.append('searchString', searchString);
    }
    return this._httpClient.get<Array<IProduct>>(this.apiUrl + '/search', { params: params });
  }

  public getCountFilter(searchString: string, shopId = 0): Observable<number> {
    let params = new HttpParams();
    if (shopId > 0) {
      params = params.append('shopId', shopId.toString());
    }
    if (searchString.length > 2) {
      params = params.append('searchString', searchString);
    }
    return this._httpClient.get<number>(this.apiUrl + '/countFilter', { params: params });
  }

  public getCount(): Observable<number> {
    return this._httpClient.get<number>(this.apiUrl + '/count');
  }

  public get(count: number = 0, offset: number = 0): Observable<Array<IProduct>> {
    let params = new HttpParams();
    params = params.append('count', count.toString());
    if (offset > 0) {
      params = params.append('offset', offset.toString());
    }
    return this._httpClient.get<Array<IProduct>>(this.apiUrl, { params: params });
  }

  public getById(id: number): Observable<IProduct> {
    return this._httpClient.get<IProduct>(this.apiUrl + '/' + id);
  }

  public put(entity: IProduct) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.put(this.apiUrl, JSON.stringify(entity), { headers: httpHeaders });
  }

  public post(entity: IProduct): Observable<IProduct> {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.post<IProduct>(this.apiUrl, JSON.stringify(entity), { headers: httpHeaders });
  }

  public delete(id: number) {
    return this._httpClient.delete(this.apiUrl + '/' + id);
  }
}
