import {Component, OnInit} from '@angular/core';
import {AnimalBoxComponent} from "./animal-box/animal-box.component";
import {RouterModule} from "@angular/router";
import { IonicModule } from '@ionic/angular';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: 'app.component.html',
  imports: [RouterModule, IonicModule],
  styleUrls: ['app.component.scss'],
})
export class AppComponent {

  showSearchBar = false;
  constructor() {}

  toggleSearchBar(){
    this.showSearchBar = !this.showSearchBar;
  }

}
