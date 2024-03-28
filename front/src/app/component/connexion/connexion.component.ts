import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ECache } from 'src/app/enums/ECache';
import { JoueurService } from 'src/app/service/joueur.service';
import { Joueur } from 'src/app/types/Joueur';
import { environment } from 'src/environments/environment';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel, MatError } from '@angular/material/form-field';
import { MatCard, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';

@Component({
    selector: 'app-connexion',
    templateUrl: './connexion.component.html',
    styleUrls: ['./connexion.component.scss'],
    standalone: true,
    imports: [MatCard, MatCardTitle, MatCardContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatCardActions, MatButton, NgIf, MatProgressSpinner]
})
export class ConnexionComponent implements OnInit
{
  form: FormGroup;
  btnClicker: boolean = false;

  constructor(
    private joueurServ: JoueurService, 
    private toastrServ: ToastrService,
    private router: Router,
    ) { }

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      Pseudo: new FormControl("", [Validators.required]),
      Mdp: new FormControl("", [Validators.required])
    });
  }

  Connexion(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;
     
    this.btnClicker = true;

    this.joueurServ.Connexion(this.form.value.Pseudo, this.form.value.Mdp).subscribe({
      next: (retour: Joueur) =>
      {
        this.btnClicker = false;

        if(retour != null)
        {
          sessionStorage.setItem(ECache.joueur, JSON.stringify(retour)); 
          environment.infoJoueur = retour;

          this.router.navigate(["/accueil"]);
        }
        else
          this.toastrServ.info(`Je ne connais pas: ${this.form.value.Pseudo} ou ton compte n'est pas activé`);
      },
      error: () => this.btnClicker = false
    });
  }
}
