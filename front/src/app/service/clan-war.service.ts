import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ClanWar } from '../types/ClanWar';

export class ClanWarService 
{
  private readonly NOM_API = `${environment.urlApi}/clanWar`;

  private http: HttpClient = inject(HttpClient);

  Lister(_idDiscord: string): Observable<ClanWar[]>
  {
    return this.http.get<ClanWar[]>(`${this.NOM_API}/lister/${_idDiscord}`);
  }

  Participer(_idClanWar: number, _idJoueur: number): Observable<boolean>
  {
    const DATA = { IdClanWar: _idClanWar, IdJoueur: _idJoueur };
    return this.http.post<boolean>(`${this.NOM_API}/participer`, DATA);
  }
}
