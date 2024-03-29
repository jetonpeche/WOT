import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { JoueurService } from 'src/app/service/joueur.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButton } from '@angular/material/button';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel, MatError } from '@angular/material/form-field';
import { Joueur } from 'src/app/types/Joueur';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-ajouter-modifier-joueur',
    templateUrl: './ajouter-modifier-joueur.component.html',
    styleUrls: ['./ajouter-modifier-joueur.component.scss'],
    standalone: true,
    imports: [MatIcon, MatDialogTitle, MatDialogContent, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatCheckbox, MatDialogActions, MatButton, MatDialogClose, MatProgressSpinner]
})
export class AjouterModifierJoueurComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker: boolean = false;

  protected joueur: Joueur;

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dialogRef: MatDialogRef<AjouterModifierJoueurComponent>,
    private joueurServ: JoueurService, 
    private toastrServ: ToastrService
    ) { }

  ngOnInit(): void 
  {
    this.joueur = this.data?.joueur;

    this.form = new FormGroup({
      Id: new FormControl<number>(this.joueur?.Id ?? 0, Validators.min(0)),
      Pseudo: new FormControl(this.joueur?.Pseudo ?? "", Validators.required),
      IdDiscord: new FormControl(this.joueur?.IdDiscord ?? "", Validators.required),
      EstAdmin: new FormControl(this.joueur?.EstAdmin ?? false),
      EstStrateur: new FormControl(this.joueur?.EstStrateur ?? false)
    });
  }

  protected Action()
  {
    if(this.form.invalid || this.btnClicker)
      return;

    this.btnClicker = true;

    if(this.joueur)
      this.ModifierJoueur();

      else
        this.AjouterJoueur();
  }

  private ModifierJoueur(): void
  {
    this.joueurServ.Modifier(this.form.value).subscribe({
      next: () =>
      {
        this.btnClicker = false;

        this.toastrServ.success("Le joueur a été modifié");

        this.form.value.ListeIdTank = this.data.joueur.ListeIdTank;
        this.dialogRef.close(this.form.value);
      },
      error: () => this.btnClicker = false
    });
  }

  private AjouterJoueur(): void
  {
    delete this.form.value.Id;
    this.form.value.IdDiscord = this.form.value.IdDiscord.toString();

    this.joueurServ.Ajouter(this.form.value).subscribe({
      next: (retour: number) =>
      {
        this.btnClicker = false;

        this.form.value.Id = retour;
        this.form.value.EstActiver = true;
        this.form.value.ListeIdTank = [];

        this.dialogRef.close(this.form.value);
      }, 
      error: () => this.btnClicker = false
    });
  }
}
