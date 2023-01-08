import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Tank } from '../types/Tank';
import { TankExport } from '../types/export/TankExport';
import { TankModifierExport } from '../types/export/TankModifierExport';

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

  Ajouter(_tank: TankExport): Observable<number>
  {
    return this.http.post<number>(`${this.NOM_API}/ajouter`, _tank);
  }

  Modifier(_tank: TankModifierExport): Observable<boolean>
  {
    return this.http.post<boolean>(`${this.NOM_API}/modifier`, _tank);
  }
}
