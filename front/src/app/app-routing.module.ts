import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccueilComponent } from './component/accueil/accueil.component';
import { ConnexionComponent } from './component/connexion/connexion.component';
import { GestionJoueurComponent } from './component/gestion-joueur/gestion-joueur.component';

const routes: Routes = [
  { path: "", component: ConnexionComponent, title: "Connexion" },
  { path: "accueil", component: AccueilComponent },
  { path: "gestion-joueur", component: GestionJoueurComponent, title: "Gestion des joueurs" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
