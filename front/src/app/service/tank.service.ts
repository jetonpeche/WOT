import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Tank } from '../types/Tank';
import { TankExport } from '../types/export/TankExport';
import { TankModifierExport } from '../types/export/TankModifierExport';
import { TankAdmin } from '../types/TankAdmin';
import { inject } from '@angular/core';

export class TankService 
{
  private readonly NOM_API = `${environment.urlApi}/tank`;

  private http: HttpClient = inject(HttpClient);

  /**
   * @param _seulementVisible false => TOUT les tanks
   */
  Lister(_seulementVisible: boolean): Observable<Tank[] | TankAdmin[]>
  {
    return this.http.get<Tank[] | TankAdmin[]>(`${this.NOM_API}/lister/${_seulementVisible}`);
  }

  Lister2(_idJoueur: number): Observable<Tank[]>
  {
    return this.http.get<Tank[]>(`${this.NOM_API}/lister/${_idJoueur}`);
  }

  Ajouter(_tank: TankExport): Observable<number>
  {
    return this.http.post<number>(`${this.NOM_API}/ajouter`, _tank);
  }

  Modifier(_tank: TankModifierExport): Observable<boolean>
  {
    return this.http.put<boolean>(`${this.NOM_API}/modifier`, _tank);
  }

  Supprimer(_idTank: number): Observable<boolean>
  {
    return this.http.delete<boolean>(`${this.NOM_API}/supprimer/${_idTank}`);
  }
}
