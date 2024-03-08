import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose, MAT_DIALOG_DATA } from '@angular/material/dialog';
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
import { TankAdmin } from 'src/app/types/TankAdmin';
import { MatIconModule } from '@angular/material/icon';

@Component({
    selector: 'app-ajouter-modifier-tank',
    templateUrl: './ajouter-modifier-tank.component.html',
    styleUrls: ['./ajouter-modifier-tank.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatIconModule, MatDialogContent, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatSelect, MatOption, MatDialogActions, MatButton, MatDialogClose, MatProgressSpinner]
})
export class AjouterModifierTankComponent implements OnInit
{
  protected form: FormGroup;
  protected listeTier: Tier[] = [];
  protected listeTypeTank: TypeTank[] = [];
  protected listeStatutTank: StatutTank[] = [];

  protected btnClicker: boolean = false;

  private tank: TankAdmin;

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private tankServ: TankService, 
    private toastrServ: ToastrService,
    private dialogRef: MatDialogRef<AjouterModifierTankComponent>) { }

  ngOnInit(): void 
  {
    this.listeTier = environment.listeTier;
    this.listeTypeTank = environment.listeTypeTank;
    this.listeStatutTank = environment.listeStatutTank;

    this.tank = this.data?.tank;

    this.form = new FormGroup({
      Nom: new FormControl(this.tank?.Nom ?? "", [Validators.required]),
      IdTier: new FormControl<number>(this.tank?.IdTier, [Validators.required]),
      IdType: new FormControl<number>(this.tank?.IdTypeTank, [Validators.required]),
      IdStatut: new FormControl<number>(this.tank?.IdStatut, [Validators.required])
    });
  }

  protected Action(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;

    this.btnClicker = true;

    if(this.tank)
      this.ModifierTank();

    else
      this.AjouterTank();    
  }

  private AjouterTank(): void
  {
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

  private ModifierTank(): void
  {
    this.tankServ.Modifier(this.form.value).subscribe({
      next: (retour: boolean) =>
      {
        this.btnClicker = false;

        if(retour)
        {
          this.toastrServ.success("Le tank a été modifié");
          this.dialogRef.close(this.form.value);
        }
        else
          this.toastrServ.error("Impossible de modifier la tank");
      },
      error: () => this.btnClicker = false
    });
  }
}
