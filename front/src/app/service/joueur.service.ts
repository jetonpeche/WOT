import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JoueurExport } from '../types/export/JoueurExport';
import { JoueurModifierExport } from '../types/export/JoueurModifierExport';
import { Joueur } from '../types/Joueur';
import { ERoleJoueur } from '../enums/ERoleJoueur';

@Injectable({
  providedIn: 'root'
})
export class JoueurService 
{
  private readonly NOM_API = `${environment.urlApi}/joueur`;

  constructor(private http: HttpClient) { }

  Lister(_roleJoueur: ERoleJoueur): Observable<Joueur[]>
  {
    return this.http.get<Joueur[]>(`${this.NOM_API}/lister/${_roleJoueur}`);
  }

  ListerPossedeTank(_idTank: number): Observable<string[]>
  {
    return this.http.get<string[]>(`${this.NOM_API}/listerPossedeTank/${_idTank}`);
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

  AjouterTankJoueur(_idTank: number): Observable<void>
  {
    const DATA = { IdTank: _idTank, IdJoueur: environment.infoJoueur.Id };
    return this.http.post<void>(`${this.NOM_API}/ajouterTankJoueur`, DATA);
  }

  Modifier(_joueur: JoueurModifierExport): Observable<boolean>
  {
    return this.http.put<boolean>(`${this.NOM_API}/modifier`, _joueur);
  }

  SupprimerTankJoueur(_idTank: number): Observable<void>
  {
    const DATA = { IdTank: _idTank, IdJoueur: environment.infoJoueur.Id };
    return this.http.put<void>(`${this.NOM_API}/supprimerTankJoueur`, DATA);
  }

  Supprimer(_idDiscord: string): Observable<boolean>
  {
    return this.http.delete<boolean>(`${this.NOM_API}/supprimer/${_idDiscord}`);
  }
}
