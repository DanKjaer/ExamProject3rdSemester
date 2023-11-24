import { Component, OnInit } from '@angular/core';
import {state} from "@angular/animations";

@Component({
  selector: 'app-animals',
  templateUrl: './animals.component.html',
  styleUrls: ['./animals.component.scss'],
})
export class AnimalsComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

  protected readonly state = state;
}
