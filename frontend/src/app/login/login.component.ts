import { Component, OnInit } from '@angular/core';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent  implements OnInit {

  constructor(public http: HttpClient) { }

  ngOnInit() {}

  async submit() {
    const url = '/http://localhost:5000/api/Login';
    var response = await firstValueFrom(this.http.post<ResponseDto<any>>(url, this.form.value));

    (await this.toast.create({
      message: response.messageToClient,
      color: "success",
      duration: 5000
    })).present();
  }
}
