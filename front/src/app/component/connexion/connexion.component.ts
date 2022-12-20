import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ECache } from 'src/app/enums/ECache';
import { JoueurService } from 'src/app/service/joueur.service';
import { Joueur } from 'src/app/types/Joueur';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-connexion',
  templateUrl: './connexion.component.html',
  styleUrls: ['./connexion.component.scss']
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
      Pseudo: new FormControl("", [Validators.required])
    });
  }

  Connexion(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;
     
    this.btnClicker = true;

    this.joueurServ.Connexion(this.form.value.Pseudo).subscribe({
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
          this.toastrServ.info(`Je ne connais pas: ${this.form.value.Pseudo}`);
      },
      error: () => this.btnClicker = false
    });
  }
}
