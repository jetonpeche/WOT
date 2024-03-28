import { Component, OnInit } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardActions, MatCardContent, MatCardTitle } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { EEtatClanWar } from 'src/app/enums/EEtatClanWar';
import { AjouterClanWarComponent } from 'src/app/modal/ajouter-clan-war/ajouter-clan-war.component';
import { ClanWarService } from 'src/app/service/clan-war.service';
import { ClanWar } from 'src/app/types/ClanWar';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-gestion-clan-war',
    templateUrl: './gestion-clan-war.component.html',
    styleUrls: ['./gestion-clan-war.component.scss'],
    imports: [MatButton, MatCard, MatCardContent, MatCardActions, MatCardTitle, MatIconModule],
    standalone: true
})
export class GestionClanWarComponent implements OnInit
{
  listeClanWar: ClanWar[] = [];

  constructor(private clanWarServ: ClanWarService, private dialog: MatDialog) { }

  ngOnInit(): void 
  {
    this.ListerClanWar();
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
