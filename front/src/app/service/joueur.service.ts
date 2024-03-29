import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JoueurExport } from '../types/export/JoueurExport';
import { JoueurModifierExport } from '../types/export/JoueurModifierExport';
import { Joueur } from '../types/Joueur';
import { ERoleJoueur } from '../enums/ERoleJoueur';
import { inject } from '@angular/core';

export class JoueurService 
{
  private readonly NOM_API = `${environment.urlApi}/joueur`;

  private http: HttpClient = inject(HttpClient);

  Lister(_roleJoueur: ERoleJoueur): Observable<Joueur[]>
  {
    return this.http.get<Joueur[]>(`${this.NOM_API}/lister/${_roleJoueur}`);
  }

  ListerPossedeTank(_idTank: number): Observable<string[]>
  {
    return this.http.get<string[]>(`${this.NOM_API}/listerPossedeTank/${_idTank}`);
  }

  Connexion(_pseudo: string, _mdp: string): Observable<Joueur>
  {
    const DATA = { Pseudo: _pseudo, Mdp: _mdp };
    return this.http.post<Joueur>(`${this.NOM_API}/connexion`, DATA);
  }

  InserverEtatActiver(_idJoueur: number): Observable<void>
  {
    return this.http.put<void>(`${this.NOM_API}/inserverEtatActiver/${_idJoueur}`, "");
  }

  Ajouter(_joueur: JoueurExport): Observable<number>
  {
    return this.http.post<number>(`${this.NOM_API}/ajouter`, _joueur);
  }

  AjouterTankJoueur(_idTank: number): Observable<void>
  {
    const DATA = { IdTank: _idTank, IdJoueur: environment.infoJoueur.Id };
    return this.http.post<void>(`${this.NOM_API}/ajouterTankJoueur`, DATA);
  }

  Modifier(_joueur: JoueurModifierExport): Observable<void>
  {
    return this.http.put<void>(`${this.NOM_API}/modifier`, _joueur);
  }

  SupprimerTankJoueur(_idTank: number): Observable<void>
  {
    const DATA = { IdTank: _idTank, IdJoueur: environment.infoJoueur.Id };
    return this.http.put<void>(`${this.NOM_API}/supprimerTankJoueur`, DATA);
  }

  Supprimer(_idDiscord: string): Observable<void>
  {
    return this.http.delete<void>(`${this.NOM_API}/supprimer/${_idDiscord}`);
  }
}
