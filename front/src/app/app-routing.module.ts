import { Routes } from '@angular/router';
import { AdminGuard } from './guard/admin.guard';
import { StrateurGuard } from './guard/strateur.guard';

export const routes: Routes = [
  { 
    path: "", 
    loadComponent: () => import("./component/connexion/connexion.component").then(x => x.ConnexionComponent), 
    title: "Connexion" 
  },
  { 
    path: "accueil", 
    loadComponent: () => import("./component/accueil/accueil.component").then(x => x.AccueilComponent) 
  },
  { 
    path: "gestion-joueur", 
    canActivate: [AdminGuard],
    loadComponent: () => import("./component/gestion-joueur/gestion-joueur.component").then(x => x.GestionJoueurComponent), 
    title: "Gestion des joueurs" 
  },
  { 
    path: "gestion-tank", 
    canActivate: [AdminGuard],
    loadComponent: () => import("./component/gestion-tank/gestion-tank.component").then(x => x.GestionTankComponent), 
    title: "Gestion des tanks"
  },
  { 
    path: "gestion-clan-war", 
    canActivate: [StrateurGuard],
    loadComponent: () => import("./component/gestion-clan-war/gestion-clan-war.component").then(x => x.GestionClanWarComponent), 
    title: "Gestion des clan war" 
  }
];
