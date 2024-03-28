import { Component, OnInit } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardActions, MatCardContent, MatCardTitle } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ToastrService } from 'ngx-toastr';
import { EEtatClanWar } from 'src/app/enums/EEtatClanWar';
import { AjouterClanWarComponent } from 'src/app/modal/ajouter-clan-war/ajouter-clan-war.component';
import { ClanWarService } from 'src/app/service/clan-war.service';
import { OutilService } from 'src/app/service/outil.service';
import { ClanWar } from 'src/app/types/ClanWar';

@Component({
    selector: 'app-gestion-clan-war',
    templateUrl: './gestion-clan-war.component.html',
    styleUrls: ['./gestion-clan-war.component.scss'],
    imports: [MatProgressSpinnerModule, MatButton, MatCard, MatCardContent, MatCardActions, MatCardTitle, MatIconModule],
    standalone: true
})
export class GestionClanWarComponent implements OnInit
{
  listeClanWar: ClanWar[] = [];
  btnClicker: boolean = false;

  constructor(
    private toastrServ: ToastrService,
    private clanWarServ: ClanWarService, 
    private outilServ: OutilService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void 
  {
    this.ListerClanWar();
  }

  protected ConfimerSupprimerClanWar(_idClanWar: number)
  {
    const TITRE = "Supprimer clan war";
    const MSG = "Confirmez vous la suppression de la clan war ?"

    this.outilServ.OuvrirModalConfirmation(TITRE, MSG);

    this.outilServ.retourConfirmation.subscribe({
      next: (retour: boolean) =>
      {
        if(retour)
          this.SupprimerClanWar(_idClanWar);
      }
    });
  }

  protected OuvrirModalAjouterClanWar(): void
  {
    const DIALOG_REF = this.dialog.open(AjouterClanWarComponent);

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: ClanWar) =>
      {
        if(retour)
          this.listeClanWar.push(retour);
      }
    });
  }

  private SupprimerClanWar(_idClanWar: number): void
  {
    this.btnClicker = true;

    this.clanWarServ.Supprimer(_idClanWar).subscribe({
      next: () => 
      {
        this.btnClicker = false;

        const INDEX = this.listeClanWar.findIndex(x => x.Id == _idClanWar);
        this.listeClanWar.splice(INDEX, 1);

        this.toastrServ.success("", "La clan a été supprimée");
      },
      error: () => this.btnClicker = false
    });
  }

  private ListerClanWar(): void
  {
    this.clanWarServ.Lister2(EEtatClanWar.Toute).subscribe({
      next: (retour: ClanWar[]) =>
      {
        console.log(retour);
        this.listeClanWar = retour;
      } 
    });
  }
}
