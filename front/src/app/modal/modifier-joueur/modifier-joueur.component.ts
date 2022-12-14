import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { JoueurService } from 'src/app/service/joueur.service';

@Component({
  selector: 'app-modifier-joueur',
  templateUrl: './modifier-joueur.component.html',
  styleUrls: ['./modifier-joueur.component.scss']
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
