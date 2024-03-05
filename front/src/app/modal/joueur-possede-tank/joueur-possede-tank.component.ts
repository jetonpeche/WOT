import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { JoueurService } from 'src/app/service/joueur.service';
import { MatButton } from '@angular/material/button';
import { NgFor } from '@angular/common';
import { MatList, MatListItem } from '@angular/material/list';

@Component({
    selector: 'app-joueur-possede-tank',
    templateUrl: './joueur-possede-tank.component.html',
    styleUrls: ['./joueur-possede-tank.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatDialogContent, MatList, NgFor, MatListItem, MatDialogActions, MatButton, MatDialogClose]
})
export class JoueurPossedeTankComponent implements OnInit
{
  protected nomTank: string;
  protected listeNomJoueur: string[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) private data: any, private joueurServ: JoueurService) { }

  ngOnInit(): void 
  {
    this.nomTank = this.data.nomTank;
    this.ListerJoueurPossedeTank(this.data.idTank);
  }

  private ListerJoueurPossedeTank(_idTank: number): void
  {
    this.joueurServ.ListerPossedeTank(_idTank).subscribe({
      next: (retour: string[]) =>
      {
        this.listeNomJoueur = retour;
      }
    });
  }
}
