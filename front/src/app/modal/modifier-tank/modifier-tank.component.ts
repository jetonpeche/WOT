import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { TankService } from 'src/app/service/tank.service';
import { StatutTank } from 'src/app/types/StatutTank';
import { Tier } from 'src/app/types/Tier';
import { TypeTank } from 'src/app/types/TypeTank';
import { environment } from 'src/environments/environment';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButton } from '@angular/material/button';
import { MatOption } from '@angular/material/core';
import { NgFor, NgIf } from '@angular/common';
import { MatSelect } from '@angular/material/select';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel, MatError } from '@angular/material/form-field';

@Component({
    selector: 'app-modifier-tank',
    templateUrl: './modifier-tank.component.html',
    styleUrls: ['./modifier-tank.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatDialogContent, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatError, MatSelect, NgFor, MatOption, MatDialogActions, MatButton, MatDialogClose, NgIf, MatProgressSpinner]
})
export class ModifierTankComponent implements OnInit
{
  protected form: FormGroup;
  protected listeTier: Tier[] = [];
  protected listeTypeTank: TypeTank[] = [];
  protected listeStatutTank: StatutTank[] = [];

  protected btnClicker: boolean = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private tankServ: TankService, 
    private toastrServ: ToastrService,
    private dialogRef: MatDialogRef<ModifierTankComponent>) { }

  ngOnInit(): void 
  {
    this.listeTier = environment.listeTier;
    this.listeTypeTank = environment.listeTypeTank;
    this.listeStatutTank = environment.listeStatutTank;

    this.form = new FormGroup({
      Nom: new FormControl(this.data.tank.Nom, [Validators.required]),
      IdTier: new FormControl<number>(this.data.tank.IdTier, [Validators.required]),
      IdType: new FormControl<number>(this.data.tank.IdTypeTank, [Validators.required]),
      IdStatut: new FormControl<number>(this.data.tank.IdStatut, [Validators.required]),

      Id: new FormControl<number>(this.data.tank.Id),
      EstVisible: new FormControl<boolean>(this.data.tank.EstVisible)
    });
  }

  ModifierTank(): void
  {
    if(this.form.invalid || this.btnClicker)
      return;

    this.btnClicker = true;

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
