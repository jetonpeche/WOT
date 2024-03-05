import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { JoueurService } from 'src/app/service/joueur.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButton } from '@angular/material/button';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel, MatError } from '@angular/material/form-field';

@Component({
    selector: 'app-modifier-joueur',
    templateUrl: './modifier-joueur.component.html',
    styleUrls: ['./modifier-joueur.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatCheckbox, MatDialogActions, MatButton, MatDialogClose, MatProgressSpinner]
})
export class ModifierJoueurComponent implements OnInit
{
  protected btnClicker: boolean = false;
  protected form: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dialogRef: MatDialogRef<ModifierJoueurComponent>, 
    private toastrServ: ToastrService,
    private joueurServ: JoueurService) { }

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      Id: new FormControl(this.data.joueur.Id, Validators.required),
      Pseudo: new FormControl(this.data.joueur.Pseudo, Validators.required),
      IdDiscord: new FormControl(this.data.joueur.IdDiscord, Validators.required),
      EstAdmin: new FormControl(this.data.joueur.EstAdmin),
      EstStrateur: new FormControl(this.data.joueur.EstStrateur)
    });
  }

  protected ModifierJoueur(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;
    
    this.btnClicker = true;

    this.joueurServ.Modifier(this.form.value).subscribe({
      next: (retour: boolean) =>
      {
        this.btnClicker = false;

        if(retour)
        {
          this.toastrServ.success("Le joueur a été modifié");

          this.form.value.ListeIdTank = this.data.joueur.ListeIdTank;
          this.dialogRef.close(this.form.value);
        }
        else
          this.toastrServ.error("Erreur impossible de modifier le compte");
      },
      error: () => this.btnClicker = false
    });
  }
}
