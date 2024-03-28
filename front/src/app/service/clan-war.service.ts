import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ClanWar } from '../types/ClanWar';
import { EEtatClanWar } from '../enums/EEtatClanWar';
import { ClanWarDetail } from '../types/ClanWarDetail';

export class ClanWarService 
{
  private readonly NOM_API = `${environment.urlApi}/clanWar`;

  private http: HttpClient = inject(HttpClient);

  Lister2(_etat: EEtatClanWar): Observable<ClanWar[]>
  {
    return this.http.get<ClanWar[]>(`${this.NOM_API}/lister/${_etat}`);
  }

  Lister(_idDiscord: string, _etat: EEtatClanWar): Observable<ClanWar[]>
  {
    return this.http.get<ClanWar[]>(`${this.NOM_API}/lister/${_idDiscord}/${_etat}`);
  }

  Detail(_idClanWar: number): Observable<ClanWarDetail>
  {
    return this.http.get<ClanWarDetail>(`${this.NOM_API}/detail/${_idClanWar}`);
  }

  Ajouter(_date): Observable<number>
  {
    const DATA = { IdDiscord: environment.infoJoueur.IdDiscord, Date: _date };
    return this.http.post<number>(`${this.NOM_API}/ajouter`, DATA);
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
