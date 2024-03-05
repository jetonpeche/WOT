import { Component, OnInit } from '@angular/core';
import { ClanWarService } from 'src/app/service/clan-war.service';
import { ClanWar } from 'src/app/types/ClanWar';
import { environment } from 'src/environments/environment';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';
import { NgFor } from '@angular/common';

@Component({
    selector: 'app-accueil',
    templateUrl: './accueil.component.html',
    styleUrls: ['./accueil.component.scss'],
    standalone: true,
    imports: [NgFor, MatCard, MatCardTitle, MatCardContent, MatCardActions, MatButton]
})
export class AccueilComponent implements OnInit
{
  listeClanWar: ClanWar[] = [];

  constructor(private clanWarServ: ClanWarService) { }

  ngOnInit(): void 
  {
    this.ListerClanWar();
  }

  protected Participer(_clanWar: ClanWar): void
  {
    this.clanWarServ.Participer(_clanWar.Id, environment.infoJoueur.Id).subscribe({
      next: (retour: boolean) =>
      {
        
      }
    });
  }

  private ListerClanWar(): void
  {
    this.clanWarServ.Lister(environment.infoJoueur.IdDiscord).subscribe({
      next: (retour: ClanWar[]) =>
      {
        this.listeClanWar = retour;
      }
    });
  }
}
