import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {State} from "../../state";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Users} from "../../models";
import {firstValueFrom} from "rxjs";
import {ModalController, ToastController} from "@ionic/angular";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-employee-update',
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.scss'],
})
export class EmployeeUpdateComponent  implements OnInit {
  updateEmployee = this.fb.group({
    userName: [this.state.selectedUser.userName, Validators.required],
    userEmail: [this.state.selectedUser.userEmail, Validators.required],
    phoneNumber: [this.state.selectedUser.phoneNumber, Validators.required],
    userType: [this.state.selectedUser.userType, Validators.required],
    toBeDisabledDate: [this.state.selectedUser.toBeDisabledDate, Validators.required]
  })

  apiUrl = 'http://localhost:5000/api/users';

  constructor(public fb: FormBuilder, public state: State, public http: HttpClient, public modalController: ModalController, public toast: ToastController, public route: ActivatedRoute) { }

  staffId?: string | null;

  ngOnInit() {
    this.getEmployeeId();
  }

  async getEmployeeId(){
    this.route.paramMap.subscribe(params => {
      this.staffId = params.get('id');
    })

    if (this.staffId !== null) {
    const result = await firstValueFrom(this.http.get<Users>(this.apiUrl + this.staffId));
    this.state.selectedUser = result;
    }
  }

  async updateEmployeeMethod(){
    try{
      let dto = this.updateEmployee.getRawValue();
      const observable = this.http.put<Users>(this.apiUrl + '/' + this.state.selectedUser.userID, dto);
      const response = await firstValueFrom(observable);
      const index = this.state.user.findIndex((user) => user.userID === this.state.selectedUser.userID);
      if(index !== -1){
        this.state.user[index] = response;
      }
      this.modalController.dismiss();
      this.sortUserList();
    }catch (e){
      if(e instanceof HttpErrorResponse){
        this.toast.create({message: e.error.messageToClient, duration: 1000}).then((res) => res.present())
      }
    }
  }

  private sortUserList() {
    console.log('before sort',this.state.currentUser)
    this.state.user = this.state.user.sort((a, b) =>{
      return a.disabled ? 1 : -1;
    });
    console.log('After sort', this.state.user)
  }

  async modalClose(){
    this.modalController.dismiss();
  }
}
