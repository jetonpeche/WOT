import { Component, OnInit } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { EEtatClanWar } from 'src/app/enums/EEtatClanWar';
import { ClanWarService } from 'src/app/service/clan-war.service';
import { ClanWar } from 'src/app/types/ClanWar';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-gestion-clan-war',
    templateUrl: './gestion-clan-war.component.html',
    styleUrls: ['./gestion-clan-war.component.scss'],
    imports: [MatButton],
    standalone: true
})
export class GestionClanWarComponent implements OnInit
{
  constructor(private clanWarServ: ClanWarService, ) { }

  ngOnInit(): void 
  {
    this.ListerClanWar();
  }

  protected OuvrirModalAjouterClanWar(): void
  {

  }

  private ListerClanWar(): void
  {
    this.clanWarServ.Lister(environment.infoJoueur.IdDiscord, EEtatClanWar.Toute).subscribe({
      next: (retour: ClanWar[]) =>
      {
        console.log(retour);
        
      } 
    })
  }
}
