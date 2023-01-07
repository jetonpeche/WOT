import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { JoueurService } from 'src/app/service/joueur.service';

@Component({
  selector: 'app-joueur-possede-tank',
  templateUrl: './joueur-possede-tank.component.html',
  styleUrls: ['./joueur-possede-tank.component.scss']
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
