import { Component, OnInit } from '@angular/core';
import {State} from "../../state";
import {ModalController, ToastController} from "@ionic/angular";
import {FormBuilder, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Users} from "../../models";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.scss'],
})
export class EmployeeCreateComponent  implements OnInit {

  constructor(public state: State, public modalController: ModalController, public formBuilder: FormBuilder, public http: HttpClient, public toast: ToastController) { }

  apiUrl = 'http://localhost:5000/api/users';
  /*http://localhost:5000/api/users?password=passwordeksempel*/

  ngOnInit() {}

  createNewEmployee = this.formBuilder.group({
    userName: ['', Validators.required],
    userEmail: ['', Validators.required],
    password: ['', Validators.required],
    phoneNumber: ['', Validators.required],
    userType: [0, Validators.required],
    /*toBeDisabledDate: ['']*/
  })

  async newEmployee(){
    try{
      let info = this.createNewEmployee.getRawValue();
      console.log('Request Data:', info);
      const observable = this.http.post<Users>(this.apiUrl + "?password=" + this.createNewEmployee.getRawValue().password, info);
      const response = await firstValueFrom(observable);
      this.state.user.push(response);
      console.log(observable)
      this.modalController.dismiss();
    } catch (e) {
      if (e instanceof HttpErrorResponse){
        this.toast.create({message: e.error.messageToClient, duration: 1000}).then((res) => res.present())
      };
    }
  }

    async modalClose(){
    this.modalController.dismiss();
    }
}

