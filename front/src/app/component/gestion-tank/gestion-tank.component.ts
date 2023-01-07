import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AjouterTankComponent } from 'src/app/modal/ajouter-tank/ajouter-tank.component';
import { TankService } from 'src/app/service/tank.service';
import { StatutTank } from 'src/app/types/StatutTank';
import { Tank } from 'src/app/types/Tank';
import { Tier } from 'src/app/types/Tier';
import { TypeTank } from 'src/app/types/TypeTank';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-gestion-tank',
  templateUrl: './gestion-tank.component.html',
  styleUrls: ['./gestion-tank.component.scss']
})
export class GestionTankComponent implements OnInit
{
  listeTank: Tank[] = [];
  listeTier: Tier[] = [];
  listeTypeTank: TypeTank[] = [];
  listeStatutTank: StatutTank[] = [];

  private listeTankClone: Tank[] = [];

  constructor(
    private dialog: MatDialog, 
    private tankServ: TankService,
    private toastrServ: ToastrService
    ) { }

  ngOnInit(): void 
  {
    this.listeTier = environment.listeTier;
    this.listeTypeTank = environment.listeTypeTank;
    this.listeStatutTank = environment.listeStatutTank;

    this.ListerTank();
  }

  protected OuvrirModalAjouterTank(): void
  {
    const DIALOG_REF = this.dialog.open(AjouterTankComponent);

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: Tank) =>
      {
        if(retour != undefined)
        {
          this.listeTank.push(retour);

          this.listeTank.sort((a, b) =>
          {
            if(a.Nom.toLowerCase() > b.Nom.toLowerCase()) 
              return 1;
            else if(a.Nom.toLowerCase() < b.Nom.toLowerCase())
              return -1;
            else
              return 0;
          });

          console.log(this.listeTank);
          console.log(this.listeTankClone);
          
          

          this.toastrServ.success("Le tank a été ajouté");
        }
      }
    });
  }

  protected OuvrirModalJoueurPossedeTank(_idTank: number)
  {

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

  protected Filtrer(_idTier: number = 0, _idType: number = 0, _idStatut: number = 0): void
  { 
    this.listeTank = this.listeTankClone;

    if(_idTier == 0 && _idType == 0 && _idStatut == 0)
      return;   

    // filtre cumulatif
    if(_idTier != 0 && _idTier != undefined)
      this.listeTank = this.listeTank.filter(x => x.IdTier == _idTier);

    if(_idType != 0 && _idTier != undefined)
      this.listeTank = this.listeTank.filter(x => x.IdTypeTank == _idType);

    if(_idStatut != 0 && _idStatut != undefined)
      this.listeTank = this.listeTank.filter(x => x.IdStatut == _idStatut);
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
    this.tankServ.Lister(true).subscribe({
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
}
