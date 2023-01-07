import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// angular material
import {MatCardModule} from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatDialogModule} from '@angular/material/dialog';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatSelectModule} from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatRadioModule} from '@angular/material/radio';
import {MatTableModule} from '@angular/material/table';
import {MatSortModule} from '@angular/material/sort';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatListModule} from '@angular/material/list';

// permet de donner la possibilité de refrech la page en mode prod en ajoutant un # sur URL
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { JwtInterceptor } from './interceptor/jwt.interceptor';

// components
import { ConnexionComponent } from './component/connexion/connexion.component';
import { AccueilComponent } from './component/accueil/accueil.component';
import { ModifierInfoCompteComponent } from './modal/modifier-info-compte/modifier-info-compte.component';
import { GestionJoueurComponent } from './component/gestion-joueur/gestion-joueur.component';
import { ConfirmationComponent } from './modal/confirmation/confirmation.component';
import { AjouterJoueurComponent } from './modal/ajouter-joueur/ajouter-joueur.component';
import { ModifierJoueurComponent } from './modal/modifier-joueur/modifier-joueur.component';
import { GestionTankComponent } from './component/gestion-tank/gestion-tank.component';
import { AjouterTankComponent } from './modal/ajouter-tank/ajouter-tank.component';
import { JoueurPossedeTankComponent } from './modal/joueur-possede-tank/joueur-possede-tank.component';

@NgModule({
  declarations: [
    AppComponent,
    ConnexionComponent,
    AccueilComponent,
    ModifierInfoCompteComponent,
    GestionJoueurComponent,
    ConfirmationComponent,
    AjouterJoueurComponent,
    ModifierJoueurComponent,
    GestionTankComponent,
    AjouterTankComponent,
    JoueurPossedeTankComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      progressBar: true,
      progressAnimation: 'increasing'
      //positionClass: 
    }),
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatToolbarModule,
    MatIconModule,
    MatSelectModule,
    MatCheckboxModule,
    MatRadioModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatListModule
  ],
  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
