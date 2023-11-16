import { Component } from '@angular/core';

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
