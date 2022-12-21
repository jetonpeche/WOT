import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { ConfirmationComponent } from '../modal/confirmation/confirmation.component';

@Injectable({
  providedIn: 'root'
})
export class OutilService 
{
  retourConfirmation: Subject<boolean>;

  constructor(private dialog: MatDialog) { }

  OuvrirModalConfirmation(_titre: string, _message: string): void
  {
    this.retourConfirmation = new Subject();

    const DIALOG_REF = this.dialog.open(ConfirmationComponent, { data: { titre: _titre, message: _message }});

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: boolean) =>
      {
        if(retour == undefined)
          retour = false;

        this.retourConfirmation.next(retour);
        this.retourConfirmation.complete();
      }
    });
  }
}
