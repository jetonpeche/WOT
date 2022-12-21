import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss']
})
export class ConfirmationComponent implements OnInit
{
  protected titre: string;
  protected message: string;

  constructor(@Inject(MAT_DIALOG_DATA) private data: any) { }

  ngOnInit(): void 
  {
    this.titre = this.data.titre;
    this.message = this.data.message;
  }
}
