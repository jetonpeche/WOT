import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { ECache } from './enums/ECache';
import { ModifierInfoCompteComponent } from './modal/modifier-info-compte/modifier-info-compte.component';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton, MatButton } from '@angular/material/button';
import { MatToolbar } from '@angular/material/toolbar';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: true,
    imports: [NgIf, MatToolbar, MatIconButton, MatIcon, MatButton, RouterLink, RouterOutlet]
})
export class AppComponent implements OnInit
{
  tailleEcran: number = window.screen.width;
  readonly TAILLE_375 = environment.tailleEcran375;

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void 
  {
    if(sessionStorage.getItem(ECache.joueur) != undefined)
      environment.infoJoueur = JSON.parse(sessionStorage.getItem(ECache.joueur));
  }

  EstConnecter(): boolean
  {
    return environment.infoJoueur != null;
  }

  OuvrirModalModifInfoCompte(): void
  {
    this.dialog.open(ModifierInfoCompteComponent);
  }
}
