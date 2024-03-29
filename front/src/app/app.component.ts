import { Component, HostBinding, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { ECache } from './enums/ECache';
import { ModifierInfoCompteComponent } from './modal/modifier-info-compte/modifier-info-compte.component';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton, MatButton } from '@angular/material/button';
import { MatToolbar } from '@angular/material/toolbar';
import { AsyncPipe, NgStyle } from '@angular/common';
import { Observable } from 'rxjs';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: true,
    imports: [MatSlideToggleModule, NgStyle, AsyncPipe, MatIcon, MatListModule, MatSidenavModule, MatToolbar, MatIconButton, MatIcon, MatButton, RouterLink, RouterOutlet]
})
export class AppComponent implements OnInit
{
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
  .pipe(
    map(result => result.matches),
    shareReplay()
  );

  tailleEcran: number = window.screen.width;
  readonly TAILLE_375 = environment.tailleEcran375;

  @HostBinding('class') className = "";
  private darkTheme = "dark-theme";
  private lightTheme = "light-theme";

  constructor(
    private dialog: MatDialog,
    private breakpointObserver: BreakpointObserver
  ) { }

  ngOnInit(): void 
  {
    if(sessionStorage.getItem(ECache.joueur) != undefined)
      environment.infoJoueur = JSON.parse(sessionStorage.getItem(ECache.joueur));

    this.className = this.lightTheme;
  }

  EstAdmin(): boolean
  {
    return environment.infoJoueur.EstAdmin;
  }

  EstStrateur(): boolean
  {
    return environment.infoJoueur.EstStrateur;
  }

  ChangerTheme(_estCheck): void
  { 
    this.className = _estCheck ? this.darkTheme : this.lightTheme;
  }

  EstOuvert(_isHandset$): boolean
  {
    if(!this.EstConnecter())
      return false;
    
    return _isHandset$;
  }

  FermerApresClick(_sidenav): void
  {
    if(_sidenav.mode == "side")
      return;

    _sidenav.toggle();
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
