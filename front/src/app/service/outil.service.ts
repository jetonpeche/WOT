import { Subject } from 'rxjs';
import { ConfirmationComponent } from '../modal/confirmation/confirmation.component';
import { MatDialog } from '@angular/material/dialog';
import { inject } from '@angular/core';

export class OutilService 
{
  retourConfirmation: Subject<boolean>;

  private dialog: MatDialog = inject(MatDialog);

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
