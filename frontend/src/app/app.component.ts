import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import { TokenService } from 'src/services/token.services';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {

  showSearchBar = false;
  constructor(private router: Router, private readonly toast: ToastController) {}
  private async showError(message: string) {
      return (await this.toast.create({
        message: message,
        duration: 5000,
        color: 'danger'
      })).present()
    }

  isLoginRoute(): boolean{
    return this.router.url.includes('/login');
  }
}
