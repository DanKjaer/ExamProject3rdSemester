import {Component, OnInit} from '@angular/core';
import {AnimalBoxComponent} from "./animal-box/animal-box.component";
import {RouterModule} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {

  showSearchBar = false;
  constructor() {}

  toggleSearchBar(){
    this.showSearchBar = !this.showSearchBar;
  }

}
