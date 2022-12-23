import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Tank } from '../types/Tank';

@Injectable({
  providedIn: 'root'
})
export class TankService 
{
  private readonly NOM_API = `${environment.urlApi}/tank`;

  constructor(private http: HttpClient) { }

  Lister(_seulementVisible: boolean): Observable<Tank[]>
  {
    return this.http.get<Tank[]>(`${this.NOM_API}/lister/${_seulementVisible}`);
  }
}
