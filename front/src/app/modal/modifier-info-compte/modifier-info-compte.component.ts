import { Component, OnInit } from '@angular/core';
import { JoueurService } from 'src/app/service/joueur.service';
import { TankService } from 'src/app/service/tank.service';
import { StatutTank } from 'src/app/types/StatutTank';
import { Tank } from 'src/app/types/Tank';
import { Tier } from 'src/app/types/Tier';
import { TypeTank } from 'src/app/types/TypeTank';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { ECache } from 'src/app/enums/ECache';

@Component({
  selector: 'app-modifier-info-compte',
  templateUrl: './modifier-info-compte.component.html',
  styleUrls: ['./modifier-info-compte.component.scss']
})
export class ModifierInfoCompteComponent implements OnInit
{
  listeTank: Tank[] = [];
  listeTier: Tier[] = [];
  listeTypeTank: TypeTank[] = [];
  listeStatutTank: StatutTank[] = [];

  private listeTankClone: Tank[] = [];

  constructor(
    private tankServ: TankService, 
    private joueurServ: JoueurService,
    private toastrServ: ToastrService
    ) { }

  ngOnInit(): void 
  {
    this.listeTier = environment.listeTier;
    this.listeTypeTank = environment.listeTypeTank;
    this.listeStatutTank = environment.listeStatutTank;

    this.ListerTank();
  }

  protected ChoisirPasChoisirTank(_tank: Tank): void
  {
    if(_tank.estPosseder)
      this.SupprimerPossession(_tank);
    else
      this.AjouterPossession(_tank);
  }

  protected Rechercher(_recherche: string): void
  {
    if(_recherche == "")
    {
      this.listeTank = this.listeTankClone;
      return;
    }

    this.listeTank = this.listeTankClone.filter(x => x.Nom.toLowerCase().includes(_recherche.toLowerCase()));
  }

  protected Filtrer(_idTier: number = 0, _idType: number = 0, _idStatut: number = 0, _estNonPosseder: boolean): void
  { 
    this.listeTank = this.listeTankClone;

    if(_idTier == 0 && _idType == 0 && _idStatut == 0 && !_estNonPosseder)
      return;

    // filtre cumulatif
    if(_idTier != 0 && _idTier != undefined)
      this.listeTank = this.listeTank.filter(x => x.IdTier == _idTier);

    if(_idType != 0 && _idTier != undefined)
      this.listeTank = this.listeTank.filter(x => x.IdTypeTank == _idType);

    if(_idStatut != 0 && _idStatut != undefined)
      this.listeTank = this.listeTank.filter(x => x.IdStatut == _idStatut);

    // si non => on sens fou
    if(_estNonPosseder)
      this.listeTank = this.listeTank.filter(x => x.estPosseder == false);
  }

  protected GetImageTypeTank(_idType: number): string
  {
    return this.listeTypeTank.find(x => x.Id == _idType).Image;
  }

  protected GetNomStatut(_idStatut: number): string
  {
    return this.listeStatutTank.find(x => x.Id == _idStatut).Nom;
  }

  protected GetNomType(_idType: number): string
  {
    return this.listeTypeTank.find(x => x.Id == _idType).Nom;
  }

  protected GetNomTier(_idTier: number): string
  {
    return this.listeTier.find(x => x.Id == _idTier).Nom;
  }

  private ListerTank(): void
  {
    this.tankServ.Lister().subscribe({
      next: (retour: Tank[]) =>
      {
        // ajout d'un attribut
        for (let element of retour) 
          element.estPosseder = false;

        // defini les tanks posseder par le joueur
        for (const element of environment.infoJoueur.ListeIdTank) 
        {
          let tank: Tank = retour.find(x => x.Id == element);

          if(tank != undefined)
            tank.estPosseder = true;
        }

        this.listeTankClone = this.listeTank = retour;
      } 
    });
  }

  private AjouterPossession(_tank: Tank): void
  {
    this.joueurServ.AjouterTankJoueur(_tank.Id).subscribe({
      next: (retour: boolean) => 
      {
        if(retour === true)
        {
          this.toastrServ.success("Le tank a été ajouté");
          _tank.estPosseder = true;
          environment.infoJoueur.ListeIdTank.push(_tank.Id);

          sessionStorage.setItem(ECache.joueur, JSON.stringify(environment.infoJoueur));
        }
        else
          this.toastrServ.error("Erreur d'ajout");
      }
    });
  }

  private SupprimerPossession(_tank: Tank): void
  {
    this.joueurServ.SupprimerTankJoueur(_tank.Id).subscribe({
      next: (retour: boolean) =>
      {
        if(retour === true)
        {
          this.toastrServ.success("Le tank a été dépossédé");
          _tank.estPosseder = false;

          const INDEX = environment.infoJoueur.ListeIdTank.findIndex(x => x == _tank.Id);
          environment.infoJoueur.ListeIdTank.splice(INDEX, 1);

          sessionStorage.setItem(ECache.joueur, JSON.stringify(environment.infoJoueur));
        }
        else
          this.toastrServ.error("Erreur de dépossédage");
      }
    });
  }
}
