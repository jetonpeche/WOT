import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { AjouterJoueurComponent } from 'src/app/modal/ajouter-joueur/ajouter-joueur.component';
import { ModifierJoueurComponent } from 'src/app/modal/modifier-joueur/modifier-joueur.component';
import { JoueurService } from 'src/app/service/joueur.service';
import { OutilService } from 'src/app/service/outil.service';
import { Joueur } from 'src/app/types/Joueur';

@Component({
  selector: 'app-gestion-joueur',
  templateUrl: './gestion-joueur.component.html',
  styleUrls: ['./gestion-joueur.component.scss']
})
export class GestionJoueurComponent implements OnInit, AfterViewInit
{
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  btnClicker: boolean = false;

  displayedColumns: string[] = ['Pseudo', 'IdDiscord', 'NbTank', 'EstStrateur', 'EstAdmin', "EstActiver", "action"];

  listeJoueur: MatTableDataSource<Joueur>;

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
    this.paginator._intl.itemsPerPageLabel = "Joueur par page";

    this.listeJoueur.paginator = this.paginator;
    this.listeJoueur.sort = this.sort;
  }

  Rechercher(event: Event): void
  {
    const filterValue = (event.target as HTMLInputElement).value;
    this.listeJoueur.filter = filterValue.trim().toLowerCase();

    if (this.listeJoueur.paginator)
      this.listeJoueur.paginator.firstPage();
  }

  ActiverDesactiverJoueur(_joueur: Joueur, _event: Event): void
  {
    _event.stopPropagation();

    if(this.btnClicker)
      return;

    if(_joueur.EstActiver)
      this.DesactiverJoueur(_joueur);
    else
      this.ActiverJoueur(_joueur);
  }

  OuvrirModalAjouterJoueur(): void
  {
    const DIALOG_REF = this.dialog.open(AjouterJoueurComponent);

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: Joueur) =>
      {
        if(retour != undefined)
        {
          this.listeJoueur.data.push(retour);
          this.toastrServ.success("Le joueur a ??t?? ajout??");
        }      
      }
    });
  }

  OuvrirModalModifierJoueur(_joueur: Joueur): void
  {
    const DIALOG_REF = this.dialog.open(ModifierJoueurComponent, { data: { joueur: _joueur }});

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

  SupprimerJoueur(_idDiscord: string, _event: Event): void
  {
    _event.stopPropagation();

    const TITRE = "Suppression de joueur";
    const MESAAGE = "Confirmez-vous la suppression du joueur ?";

    this.outilServ.OuvrirModalConfirmation(TITRE, MESAAGE);

    this.outilServ.retourConfirmation.subscribe({
      next: (retour: boolean) =>
      {
        if(!retour)
          return;

        this.joueurServ.Supprimer(_idDiscord).subscribe({
          next: (retour: boolean) =>
          {
            if(retour)
            {
              const INDEX = this.listeJoueur.data.findIndex(x => x.IdDiscord == _idDiscord);
              this.listeJoueur.data.splice(INDEX, 1);
              this.listeJoueur.data = this.listeJoueur.data;
    
              this.toastrServ.success("Le joueur a ??t?? supprim??");
            }
            else
              this.toastrServ.error("Erreur le joueur n'a pas pu ??tre supprim??");
          }
        });
      }
    });
  }

  private ActiverJoueur(_joueur: Joueur): void
  {
    this.joueurServ.Desactiver(_joueur.Id).subscribe({
      next: (retour: boolean) =>
      {
        if(retour)
        {
          this.toastrServ.success("Le joueur est activ??");
          _joueur.EstActiver = true;
        }
        else
          this.toastrServ.error("Erreur impossible d'activer le joueur");
      }
    });
  }

  private DesactiverJoueur(_joueur: Joueur): void
  {
    this.joueurServ.Desactiver(_joueur.Id).subscribe({
      next: (retour: boolean) =>
      {
        if(retour)
        {
          this.toastrServ.success("Le joueur est d??sactiv??");
          _joueur.EstActiver = false;
        }
        else
          this.toastrServ.error("Erreur impossible de d??sactiver le joueur");
      }
    });
  }

  private ListerJoueur(): void
  {
    this.joueurServ.Lister().subscribe({
      next: (retour: Joueur[]) => 
      {
        console.log(retour);
        
        this.listeJoueur.data = retour;
      }
    });
  }
}
