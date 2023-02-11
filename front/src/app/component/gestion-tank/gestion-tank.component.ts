import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AjouterTankComponent } from 'src/app/modal/ajouter-tank/ajouter-tank.component';
import { JoueurPossedeTankComponent } from 'src/app/modal/joueur-possede-tank/joueur-possede-tank.component';
import { ModifierTankComponent } from 'src/app/modal/modifier-tank/modifier-tank.component';
import { OutilService } from 'src/app/service/outil.service';
import { TankService } from 'src/app/service/tank.service';
import { TankModifierExport } from 'src/app/types/export/TankModifierExport';
import { StatutTank } from 'src/app/types/StatutTank';
import { TankAdmin } from 'src/app/types/TankAdmin';
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
  listeTank: TankAdmin[] = [];
  listeTier: Tier[] = [];
  listeTypeTank: TypeTank[] = [];
  listeStatutTank: StatutTank[] = [];

  tailleEcran: number = window.screen.width;

  readonly TAILLE_375 = environment.tailleEcran375;

  private listeTankClone: TankAdmin[] = [];

  constructor(
    private dialog: MatDialog, 
    private tankServ: TankService,
    private outilServ: OutilService,
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
      next: (retour: TankAdmin) =>
      {
        if(retour != undefined)
        {
          this.listeTank.push(retour);

          // tri par nom du tank
          this.listeTank.sort((a, b) =>
          {
            if(a.Nom.toLowerCase() > b.Nom.toLowerCase()) 
              return 1;
            else if(a.Nom.toLowerCase() < b.Nom.toLowerCase())
              return -1;
            else
              return 0;
          });

          this.toastrServ.success("Le tank a été ajouté");
        }
      }
    });
  }

  protected OuvrirModalJoueurPossedeTank(_idTank: number, _nomTank: string): void
  {
    this.dialog.open(JoueurPossedeTankComponent, { data: { idTank: _idTank, nomTank: _nomTank }});
  }

  protected OuvrirModalJModifierTank(_tank: TankAdmin): void
  {
    const DIALOG_REF = this.dialog.open(ModifierTankComponent, { data: {tank: _tank }});

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: TankModifierExport) =>
      {
        if(retour != undefined)
        {
          _tank.Nom = retour.Nom;
          _tank.IdStatut = retour.IdStatut;
          _tank.IdTier = retour.IdTier;
          _tank.IdTypeTank = retour.IdType;
        }
      }
    });
  }

  protected OuvrirModalConfirmation(_idTank: number, _nomTank: string): void
  {
    const TITRE = "Confirmation suppression de tank";
    const MESSAGE = `Confimez vous la suppression du tank ${_nomTank} ?`;

    this.outilServ.OuvrirModalConfirmation(TITRE, MESSAGE);

    this.outilServ.retourConfirmation.subscribe({
      next: (retour: boolean) =>
      {
        if(retour === true)
          this.SupprimerTank(_idTank);
      }
    });
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

  protected Filtrer(_idTier: number = 0, _idType: number = 0, _idStatut: number = 0, _tankEstPossederParJoueur: boolean): void
  { 
    this.listeTank = this.listeTankClone;

    if(_tankEstPossederParJoueur)
      this.listeTank = this.listeTank.filter(x => x.NbPossesseur > 0)

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
    this.tankServ.Lister(false).subscribe({
      next: (retour: TankAdmin[]) =>
      {
        this.listeTankClone = this.listeTank = retour;
      } 
    });
  }

  private SupprimerTank(_idTank: number): void
  {
    this.tankServ.Supprimer(_idTank).subscribe({
      next: (retour: boolean) =>
      {
        if(retour === true)
        {
          this.toastrServ.success("Le tank a été supprimé");

          const INDEX = this.listeTank.findIndex(t => t.Id == _idTank);
          this.listeTank.splice(INDEX, 1);
        }
        else
          this.toastrServ.error("Une erreur à eu lieu");
      }
    });
  }
}
