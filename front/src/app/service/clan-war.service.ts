import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ClanWar } from '../types/ClanWar';
import { EEtatClanWar } from '../enums/EEtatClanWar';

export class ClanWarService 
{
  private readonly NOM_API = `${environment.urlApi}/clanWar`;

  private http: HttpClient = inject(HttpClient);

  Lister(_idDiscord: string, _etat: EEtatClanWar): Observable<ClanWar[]>
  {
    return this.http.get<ClanWar[]>(`${this.NOM_API}/lister/${_idDiscord}/${_etat}`);
  }

  Participer(_date: string, _idDiscord: string): Observable<void>
  {
    const DATA = { Date: _date, IdDiscord: _idDiscord };
    return this.http.post<void>(`${this.NOM_API}/participer`, DATA);
  }

  Desinscrire(_date: string, _idDiscord: string): Observable<void>
  {
    const DATA = { Date: _date, IdDiscord: _idDiscord };
    return this.http.delete<void>(`${this.NOM_API}/desinscrire`, { body: DATA });
  }
}
