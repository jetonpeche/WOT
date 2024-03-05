import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { JoueurService } from 'src/app/service/joueur.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel, MatError } from '@angular/material/form-field';

@Component({
    selector: 'app-ajouter-joueur',
    templateUrl: './ajouter-joueur.component.html',
    styleUrls: ['./ajouter-joueur.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatCheckbox, MatDialogActions, MatButton, MatDialogClose, NgIf, MatProgressSpinner]
})
export class AjouterJoueurComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker: boolean = false;

  constructor(
    private dialogRef: MatDialogRef<AjouterJoueurComponent>,
    private joueurServ: JoueurService, 
    private toastrServ: ToastrService
    ) { }

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      Pseudo: new FormControl("", Validators.required),
      IdDiscord: new FormControl("", Validators.required),
      EstAdmin: new FormControl(false),
      EstStrateur: new FormControl(false)
    });
  }

  protected AjouterJoueur(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;

    this.btnClicker = true;
    this.form.value.IdDiscord = this.form.value.IdDiscord.toString();

    this.joueurServ.Ajouter(this.form.value).subscribe({
      next: (retour: number) =>
      {
        this.btnClicker = false;

        if(retour == -1)
          this.toastrServ.error("L'id discord existe déjà");
        else if(retour == 0)
          this.toastrServ.error("Erreur l'hors de l'ajout");
        else
        {
          this.form.value.Id = retour;
          this.form.value.EstActiver = true;
          this.form.value.ListeIdTank = [];

          this.dialogRef.close(this.form.value);
        }
      }, 
      error: () => this.btnClicker = false
    });
  }
}
