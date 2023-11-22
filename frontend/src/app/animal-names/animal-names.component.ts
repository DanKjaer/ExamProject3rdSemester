import { Component, OnInit } from '@angular/core';
import {state} from "@angular/animations";
import {State} from "../../state";

@Component({
  selector: 'app-animal-names',
  templateUrl: './animal-names.component.html',
  styleUrls: ['./animal-names.component.scss'],
})
export class AnimalNamesComponent  implements OnInit {

  constructor(public state: State) { }

  ngOnInit() {}

}
