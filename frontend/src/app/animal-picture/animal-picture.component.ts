import { Component, OnInit } from '@angular/core';
import {State} from "../../state";

@Component({
  selector: 'app-animal-picture',
  templateUrl: './animal-picture.component.html',
  styleUrls: ['./animal-picture.component.scss'],
})
export class AnimalPictureComponent  implements OnInit {

  constructor(public state: State) { }

  ngOnInit() {}

}
