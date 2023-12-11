import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {State} from "../../state";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Users} from "../../models";
import {firstValueFrom} from "rxjs";
import {ModalController, ToastController} from "@ionic/angular";

@Component({
  selector: 'app-employee-update',
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.scss'],
})
export class EmployeeUpdateComponent  implements OnInit {
  updateEmployee = this.fb.group({
    userName: [this.state.currentUser.userName, Validators.required],
    userEmail: [this.state.currentUser.userEmail, Validators.required],
    phoneNumber: [this.state.currentUser.phoneNumber, Validators.required],
    userType: [this.state.currentUser.userType, Validators.required],
    toBeDisabledDate: [this.state.currentUser.toBeDisabledDate, Validators.required]
  })

  apiUrl = 'http://localhost:5000/api/users';

  constructor(public fb: FormBuilder, public state: State, public http: HttpClient, public modalController: ModalController, public toast: ToastController) { }

  ngOnInit() {}

  async updateEmployeeMethod(){
    try{
      let dto = this.updateEmployee.getRawValue();
      const observable = this.http.put<Users>(this.apiUrl + this.state.currentUser.userID, dto);
      const response = await firstValueFrom(observable);
      this.state.user = this.state.user.filter(user => user.userID != this.state.currentUser.userID);
      this.state.user.push(response);
      this.modalController.dismiss
    }catch (e){
      if(e instanceof HttpErrorResponse){
        this.toast.create({message: e.error.messageToClient, duration: 1000}).then((res) => res.present())
      }
    }
  }

  async modalClose(){
    this.modalController.dismiss();
  }
}
