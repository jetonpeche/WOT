import { Component, OnInit } from '@angular/core';
import { ClanWarService } from 'src/app/service/clan-war.service';
import { ClanWar } from 'src/app/types/ClanWar';
import { environment } from 'src/environments/environment';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';
import { EEtatClanWar } from 'src/app/enums/EEtatClanWar';
import { MatDialog } from '@angular/material/dialog';
import { ClanWarDetail } from 'src/app/types/ClanWarDetail';
import { ListerJoueurComponent } from 'src/app/modal/lister-joueur/lister-joueur.component';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-accueil',
    templateUrl: './accueil.component.html',
    styleUrls: ['./accueil.component.scss'],
    standalone: true,
    imports: [MatIcon, MatCard, MatCardTitle, MatCardContent, MatCardActions, MatButton]
})
export class AccueilComponent implements OnInit
{
  listeClanWar: ClanWar[] = [];

  constructor(
    private clanWarServ: ClanWarService, 
    private dialog: MatDialog
    ) { }

  ngOnInit(): void 
  {
    this.ListerClanWar(EEtatClanWar.Toute);
  }

  protected Action(_clanWar: ClanWar)
  {
    if(_clanWar.Participe)
      this.Desinscrire(_clanWar);
    
    else
      this.Participer(_clanWar);
  }

  protected OuvrirModalConfigClanWar(_clanWar: ClanWar): void
  {

  }

  protected OuvrirModalParticipant(_clanWar: ClanWar): void
  {
    this.clanWarServ.Detail(_clanWar.Id).subscribe({
      next: (retour: ClanWarDetail) => 
      {
        let liste: string[] = []
        for (const element of retour.ListePersonne)
          liste.push(element.Pseudo);

        this.dialog.open(ListerJoueurComponent, { data: { listeNomJoueur: liste }});
      }
    })
  }

  protected Participer(_clanWar: ClanWar): void
  {
    this.clanWarServ.Participer(_clanWar.Date, environment.infoJoueur.IdDiscord).subscribe({
      next: () =>
      {
        _clanWar.Participe = true;
        _clanWar.NbParticipant++;
      }
    });
  }

  protected Desinscrire(_clanWar: ClanWar)
  {
    this.clanWarServ.Desinscrire(_clanWar.Date, environment.infoJoueur.IdDiscord).subscribe({
      next: () =>
      {
        _clanWar.Participe = false;
        _clanWar.NbParticipant--;
      }
    });
  }

  private ListerClanWar(_etat: EEtatClanWar): void
  {
    this.clanWarServ.Lister(environment.infoJoueur.IdDiscord, _etat).subscribe({
      next: (retour: ClanWar[]) =>
      {
        this.listeClanWar = retour;
        console.log(retour);
      }
    });
  }
}
