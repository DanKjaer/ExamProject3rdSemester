import { Component, OnInit } from '@angular/core';
import {State} from "../../state";

@Component({
  selector: 'app-animal-information',
  templateUrl: './animal-information.component.html',
  styleUrls: ['./animal-information.component.scss'],
})
export class AnimalInformationComponent  implements OnInit {

  constructor(public state: State) { }

  ngOnInit() {}

}
