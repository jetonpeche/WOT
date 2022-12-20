import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Joueur } from '../types/Joueur';

@Injectable({
  providedIn: 'root'
})
export class JoueurService 
{
  private readonly NOM_API = `${environment.urlApi}/joueur`;

  constructor(private http: HttpClient) { }

  Connexion(_pseudo: string): Observable<Joueur>
  {
    return this.http.get<Joueur>(`${this.NOM_API}/info/${_pseudo}`);
  }

  AjouterTankJoueur(_idTank: number): Observable<boolean>
  {
    const DATA = { IdTank: _idTank, IdJoueur: environment.infoJoueur.Id };
    return this.http.post<boolean>(`${this.NOM_API}/ajouterTankJoueur`, DATA);
  }

  SupprimerTankJoueur(_idTank: number): Observable<boolean>
  {
    const DATA = { IdTank: _idTank, IdJoueur: environment.infoJoueur.Id };
    return this.http.post<boolean>(`${this.NOM_API}/supprimerTankJoueur`, DATA);
  }
}
