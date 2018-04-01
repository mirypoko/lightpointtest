import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { IShop } from '../models/shop';
import { IShopMode } from '../models/shopMode';

@Injectable()
export class ShopsService {

  protected apiUrl = 'api/shops';

  constructor(protected _httpClient: HttpClient) { }

  public getShopMods(): Observable<Array<IShopMode>> {
    return this._httpClient.get<Array<IShopMode>>('api/ShopModes');
  }

  public getCount(): Observable<number> {
    return this._httpClient.get<number>(this.apiUrl + '/count');
  }

  public get(count: number = 0, offset: number = 0): Observable<Array<IShop>> {
    let params = new HttpParams();
    if (offset > 0) {
      params = params.append('offset', offset.toString());
    }
    if (count > 0) {
      params = params.append('count', count.toString());
    }
    return this._httpClient.get<Array<IShop>>(this.apiUrl, { params: params });
  }

  public getById(id: number): Observable<IShop> {
    return this._httpClient.get<IShop>(this.apiUrl + '/' + id);
  }

  public put(entity: IShop) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.put(this.apiUrl, JSON.stringify(entity), { headers: httpHeaders });
  }

  public post(entity: IShop): Observable<IShop> {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.post<IShop>(this.apiUrl, JSON.stringify(entity), { headers: httpHeaders });
  }

  public delete(id: number) {
    return this._httpClient.delete(this.apiUrl + '/' + id);
  }

}
