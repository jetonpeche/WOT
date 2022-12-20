import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccueilComponent } from './component/accueil/accueil.component';
import { ConnexionComponent } from './component/connexion/connexion.component';

const routes: Routes = [
  { path: "", component: ConnexionComponent },
  { path: "accueil", component: AccueilComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
