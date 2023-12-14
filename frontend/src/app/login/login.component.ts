import { Component, OnInit } from '@angular/core';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {FormBuilder, Validators } from '@angular/forms';
import { TokenService } from 'src/services/token.services';
import { LoginResponse } from 'src/models';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent  implements OnInit {

  constructor(public http: HttpClient,
              private readonly token: TokenService,
              private readonly fb: FormBuilder,
              private readonly router: Router,
              private readonly toast: ToastController,) {}

  readonly form = this.fb.group({
    Email: ['', [Validators.required, Validators.email]],
    Password: ['', Validators.required],
  });

  ngOnInit() {}

  async submit() {
    const url = 'http://localhost:5000/api/login';
    var response = await firstValueFrom(this.http.post<LoginResponse>(url, this.form.value));
    this.token.setToken(response.token);
    if(response.token === "Failure to Authenticate")
    {
      (await this.toast.create({
        message: "Login failed",
        color: "danger",
        duration: 5000
      })).present();
    }
    else {
      this.router.navigateByUrl('/');
      (await this.toast.create({
        message: "Login successfull",
        color: "success",
        duration: 5000
      })).present();
    }
  }

  ResetPassword() {
    this.router.navigateByUrl('/resetpassword');
  }
}
