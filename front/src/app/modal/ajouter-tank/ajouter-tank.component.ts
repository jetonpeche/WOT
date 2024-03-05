import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { TankService } from 'src/app/service/tank.service';
import { StatutTank } from 'src/app/types/StatutTank';
import { Tier } from 'src/app/types/Tier';
import { TypeTank } from 'src/app/types/TypeTank';
import { environment } from 'src/environments/environment';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButton } from '@angular/material/button';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel, MatError } from '@angular/material/form-field';

@Component({
    selector: 'app-ajouter-tank',
    templateUrl: './ajouter-tank.component.html',
    styleUrls: ['./ajouter-tank.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatSelect, MatOption, MatDialogActions, MatButton, MatDialogClose, MatProgressSpinner]
})
export class AjouterTankComponent implements OnInit
{
  protected form: FormGroup;
  protected listeTier: Tier[] = [];
  protected listeTypeTank: TypeTank[] = [];
  protected listeStatutTank: StatutTank[] = [];

  protected btnClicker: boolean = false;

  constructor(
    private tankServ: TankService, 
    private toastrServ: ToastrService,
    private dialogRef: MatDialogRef<AjouterTankComponent>) { }

  ngOnInit(): void 
  {
    this.listeTier = environment.listeTier;
    this.listeTypeTank = environment.listeTypeTank;
    this.listeStatutTank = environment.listeStatutTank;

    this.form = new FormGroup({
      Nom: new FormControl("", [Validators.required]),
      IdTier: new FormControl<number>(null, [Validators.required]),
      IdType: new FormControl<number>(null, [Validators.required]),
      IdStatut: new FormControl<number>(null, [Validators.required])
    });
  }

  protected AjouterTank(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;

    this.btnClicker = true;
    console.log(this.form.value);
    
    this.tankServ.Ajouter(this.form.value).subscribe({
      next: (retour: number) =>
      {
        this.btnClicker = false;

        if(retour == 0)
          this.toastrServ.error("Ajout impossible");
        else
        {
          this.form.value.Id = retour;
          this.form.value.EstVisible = true;
          this.form.value.nbPossesseur = 0;

          this.dialogRef.close(this.form.value);
        }
      },
      error: () => this.btnClicker = false
    });
  }
}
