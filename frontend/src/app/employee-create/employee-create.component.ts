import { Component, OnInit } from '@angular/core';
import {State} from "../../state";
import {ModalController, ToastController} from "@ionic/angular";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Users} from "../../models";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.scss'],
})
export class EmployeeCreateComponent  implements OnInit {
  roleForm: FormGroup;

  constructor(public state: State, public modalController: ModalController, public formBuilder: FormBuilder, public http: HttpClient, public toast: ToastController) {
    this.roleForm = this.formBuilder.group({
      userType: this.createNewEmployee.get('userType'),
      toBeDisabledDate: new FormControl(''),
    })
  }

  apiUrl = 'http://localhost:5000/api/users';

  ngOnInit() {}

  createNewEmployee = this.formBuilder.group({
    userName: ['', Validators.required],
    userEmail: ['', Validators.required],
    password: ['', Validators.required],
    phoneNumber: ['', Validators.required],
    userType: [0, Validators.required],
    toBeDisabledDate: ['']
  })

  async newEmployee(){
    try{
      let dto = {
        ...this.createNewEmployee.getRawValue(),
        ...this.roleForm.getRawValue()
      };
      dto.toBeDisabledDate = this.roleForm.get('toBeDisabledDate')?.value || null;
      console.log('Request Data:', dto);
      const observable = this.http.post<Users>(this.apiUrl + "?password=" + this.createNewEmployee.getRawValue().password, dto);
      const response = await firstValueFrom(observable);
      this.state.user.push(response);
      console.log(observable)
      this.modalController.dismiss();
    } catch (e) {
      if (e instanceof HttpErrorResponse){
        this.toast.create({message: e.error.messageToClient, duration: 1000}).then((res) => res.present())
      }
    }
  }

    async modalClose(){
    this.modalController.dismiss();
    }
}

