import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatDateFormats, MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {provideNativeDateAdapter} from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ClanWarService } from 'src/app/service/clan-war.service';
import { ClanWar } from 'src/app/types/ClanWar';

@Component({
  selector: 'app-ajouter-clan-war',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [MatProgressSpinnerModule, MatIconModule, MatDatepickerModule, MatDialogModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './ajouter-clan-war.component.html',
  styleUrl: './ajouter-clan-war.component.scss'
})
export class AjouterClanWarComponent
{
  form: FormGroup;
  btnClicker: boolean = false;
  date = new Date();

  constructor(
    private clanWarServ: ClanWarService,
    private dialogRef: MatDialogRef<AjouterClanWarComponent>
  ) { }

  protected Ajouter(_date: string)
  {
    if(!_date)
      return;

    let dateFormater = _date.split("/").reverse().join("-");

    this.clanWarServ.Ajouter(dateFormater).subscribe({
      next: (retour: number) =>
      {
        let info: ClanWar =
        {
          Id: retour,
          Date: _date,
          NbParticipant: 0,
          Participe: false
        };

        this.dialogRef.close(info); 
      }
    });
    
  }
}
