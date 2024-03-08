import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource, MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow, MatNoDataRow } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { ERoleJoueur } from 'src/app/enums/ERoleJoueur';
import { TankJoueurComponent } from 'src/app/modal/tank-joueur/tank-joueur.component';
import { JoueurService } from 'src/app/service/joueur.service';
import { OutilService } from 'src/app/service/outil.service';
import { Joueur } from 'src/app/types/Joueur';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatTooltip } from '@angular/material/tooltip';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatButton, MatMiniFabButton } from '@angular/material/button';
import { AjouterModifierJoueurComponent } from 'src/app/modal/ajouter-modifier-joueur/ajouter-modifier-joueur.component';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-gestion-joueur',
    templateUrl: './gestion-joueur.component.html',
    styleUrls: ['./gestion-joueur.component.scss'],
    standalone: true,
    imports: [MatIcon, MatButton, MatFormField, MatLabel, MatInput, MatTable, MatSort, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatSortHeader, MatCellDef, MatCell, MatMiniFabButton, MatTooltip, MatProgressSpinner, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow, MatNoDataRow, MatPaginator, MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions]
})
export class GestionJoueurComponent implements OnInit, AfterViewInit
{
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  btnClicker: boolean = false;

  displayedColumns: string[] = ['Pseudo', 'IdDiscord', 'NbTank', 'EstStrateur', 'EstAdmin', "EstActiver", "action"];

  listeJoueur: MatTableDataSource<Joueur>;
  listeJoueurClone: Joueur[] = [];

  tailleEcran = window.screen.width;
  readonly TAILLE_768 = 768;

  constructor(
    private joueurServ: JoueurService, 
    private toastrServ: ToastrService,
    private outilServ: OutilService,
    private dialog: MatDialog
    ) { }

  ngOnInit(): void 
  {
    this.listeJoueur = new MatTableDataSource();
    this.ListerJoueur();
  }

  ngAfterViewInit(): void 
  {
    if(this.tailleEcran > this.TAILLE_768)
    {
      this.paginator._intl.itemsPerPageLabel = "Joueur par page";

      this.listeJoueur.paginator = this.paginator;
      this.listeJoueur.sort = this.sort;
    }
  }

  protected Rechercher(event: Event): void
  {
    const filterValue = (event.target as HTMLInputElement).value;

    if(this.tailleEcran > this.TAILLE_768)
    {
      this.listeJoueur.filter = filterValue.trim().toLowerCase();

      if (this.listeJoueur.paginator)
        this.listeJoueur.paginator.firstPage();
    }
    else
    {
      if(filterValue.trim() == "")
      {
        this.listeJoueur.data = this.listeJoueurClone;
        return;
      }

      this.listeJoueur.data = this.listeJoueurClone.filter(j => j.Pseudo.trim().toLowerCase().includes(filterValue));
    }
  }

  protected ActiverDesactiverJoueur(_joueur: Joueur, _event: Event): void
  {
    _event.stopPropagation();

    if(this.btnClicker)
      return;

    this.btnClicker = true;

    this.joueurServ.InserverEtatActiver(_joueur.Id).subscribe({
      next: () =>
      {
        this.btnClicker = false;
        
        _joueur.EstActiver = !_joueur.EstActiver;
        this.toastrServ.success(`Le compte est ${_joueur.EstActiver ? 'activé' : 'désactivé' }`);
      },
      error: () => this.btnClicker = false
    });
  }

  protected OuvrirModalAjouterJoueur(): void
  {
    const DIALOG_REF = this.dialog.open(AjouterModifierJoueurComponent);

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: Joueur) =>
      {
        if(retour != undefined)
        {
          this.listeJoueur.data.push(retour);
          this.listeJoueur.data = this.listeJoueur.data;
          this.toastrServ.success("Le joueur a été ajouté");
        }      
      }
    });
  }

  protected OuvrirModalModifierJoueur(_joueur: Joueur): void
  {
    const DIALOG_REF = this.dialog.open(AjouterModifierJoueurComponent, { data: { joueur: _joueur }});

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: Joueur) =>
      {
        if(retour != undefined)
        {
          _joueur.IdDiscord = retour.IdDiscord;
          _joueur.Pseudo = retour.Pseudo;
          _joueur.EstAdmin = retour.EstAdmin;
          _joueur.EstStrateur = retour.EstStrateur;
        }
      }
    });
  }

  protected ModalOuvrirModalTankJoueur(_idJoueur: number, _nomJoueur: string, _event: Event): void
  {
    _event.stopPropagation();

    this.dialog.open(TankJoueurComponent, { data: { idJoueur: _idJoueur, nomJoueur: _nomJoueur }});
  }

  protected SupprimerJoueur(_idDiscord: string, _event: Event): void
  {
    _event.stopPropagation();

    const TITRE = "Suppression de joueur";
    const MESAAGE = "Confirmez-vous la suppression du joueur ?";

    this.outilServ.OuvrirModalConfirmation(TITRE, MESAAGE);

    this.outilServ.retourConfirmation.subscribe({
      next: (retour: boolean) =>
      {
        if(!retour || this.btnClicker)
          return;

        this.btnClicker = true;

        this.joueurServ.Supprimer(_idDiscord).subscribe({
          next: () =>
          {
            this.btnClicker = false;

            const INDEX = this.listeJoueur.data.findIndex(x => x.IdDiscord == _idDiscord);
            this.listeJoueur.data.splice(INDEX, 1);
            this.listeJoueur.data = this.listeJoueur.data;
  
            this.toastrServ.success("Le joueur a été supprimé");
          },
          error: () => this.btnClicker = false
        });
      }
    });
  }

  private ListerJoueur(): void
  {
    this.joueurServ.Lister(ERoleJoueur.Tous).subscribe({
      next: (retour: Joueur[]) => 
      {
        console.log(retour);
        
        this.listeJoueurClone = this.listeJoueur.data = retour;
      }
    });
  }
}
