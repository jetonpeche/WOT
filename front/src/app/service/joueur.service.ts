import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JoueurExport } from '../types/export/JoueurExport';
import { Joueur } from '../types/Joueur';

@Injectable({
  providedIn: 'root'
})
export class JoueurService 
{
  private readonly NOM_API = `${environment.urlApi}/joueur`;

  constructor(private http: HttpClient) { }

  Lister(): Observable<Joueur[]>
  {
    return this.http.get<Joueur[]>(`${this.NOM_API}/lister`);
  }

  Connexion(_pseudo: string): Observable<Joueur>
  {
    return this.http.get<Joueur>(`${this.NOM_API}/info/${_pseudo}`);
  }

  Activer(_idJoueur: number): Observable<boolean>
  {
    return this.http.post<boolean>(`${this.NOM_API}/activer/${_idJoueur}`, "");
  }

  Desactiver(_idJoueur: number): Observable<boolean>
  {
    return this.http.post<boolean>(`${this.NOM_API}/desactiver/${_idJoueur}`, "");
  }

  /**
   * Ajouter un nouveau joueur
   * @returns 0 => erreur / -1 => id discord existe déjà / Autre => OK
   */
  Ajouter(_joueur: JoueurExport): Observable<number>
  {
    return this.http.post<number>(`${this.NOM_API}/ajouter`, _joueur);
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

  Supprimer(_idDiscord: string): Observable<boolean>
  {
    return this.http.get<boolean>(`${this.NOM_API}/supprimer/${_idDiscord}`);
  }
}
