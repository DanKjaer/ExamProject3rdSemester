import {Component} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {


  showSearchBar = false;
  constructor(private router: Router) {}

  isLoginRoute(): boolean{
    return this.router.url.includes('/login');
  }
}
