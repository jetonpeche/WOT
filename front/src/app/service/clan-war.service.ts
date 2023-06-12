import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ClanWar } from '../types/ClanWar';

@Injectable({
  providedIn: 'root'
})
export class ClanWarService 
{
  private readonly NOM_API = `${environment.urlApi}/clanWar`;

  constructor(private http: HttpClient) { }

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
