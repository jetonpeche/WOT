import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';
import { TankService } from 'src/app/service/tank.service';
import { Tank } from 'src/app/types/Tank';
import { MatButton } from '@angular/material/button';
import { NgFor } from '@angular/common';
import { MatList, MatListItem } from '@angular/material/list';

@Component({
    selector: 'app-tank-joueur',
    templateUrl: './tank-joueur.component.html',
    styleUrls: ['./tank-joueur.component.scss'],
    standalone: true,
    imports: [MatDialogTitle, MatDialogContent, MatList, NgFor, MatListItem, MatDialogActions, MatButton, MatDialogClose]
})
export class TankJoueurComponent implements OnInit
{
  protected listeNomTank: string[] = [];
  protected nomJoueur: string;

  constructor(@Inject(MAT_DIALOG_DATA) private data: any, private tankServ: TankService) { }

  ngOnInit(): void 
  {
    this.nomJoueur = this.data.nomJoueur;
    this.ListerTankJoueur();
  }

  private ListerTankJoueur(): void
  {
    this.tankServ.Lister2(this.data.idJoueur).subscribe({
      next: (retour: Tank[]) =>
      {
        for (const element of retour)
          this.listeNomTank.push(element.Nom);
      }
    });
  }
}
