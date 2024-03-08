import { Component, Inject, OnInit } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogTitle } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { MatList, MatListItem } from '@angular/material/list';

@Component({
  selector: 'app-lister-joueur',
  standalone: true,
  imports: [MatIcon, MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent, MatList, MatListItem, MatButton],
  templateUrl: './lister-joueur.component.html',
  styleUrl: './lister-joueur.component.scss'
})
export class ListerJoueurComponent implements OnInit
{
  protected listeNomJoueur: string[] = [];
  protected titre: string;

  constructor(@Inject(MAT_DIALOG_DATA) private data: any) { }

  ngOnInit(): void 
  {
    this.titre = this.data.titre;
    this.listeNomJoueur = this.data.listeNomJoueur;  
  }
}
