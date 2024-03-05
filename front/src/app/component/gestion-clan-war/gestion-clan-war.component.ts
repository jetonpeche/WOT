import { Component } from '@angular/core';
import { ClanWarService } from 'src/app/service/clan-war.service';

@Component({
    selector: 'app-gestion-clan-war',
    templateUrl: './gestion-clan-war.component.html',
    styleUrls: ['./gestion-clan-war.component.scss'],
    standalone: true
})
export class GestionClanWarComponent 
{
  constructor(private clanWarServ: ClanWarService) { }
}
