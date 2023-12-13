import { Component, OnInit } from '@angular/core';
import {State} from "../../state";
import {ModalController, ToastController} from "@ionic/angular";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Users, UserType} from "../../models";
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
    userType: [UserType, Validators.required],
    toBeDisabledDate: ['']
  })

  async newEmployee(){
    try{
      // Making the new User as an object so we can sent it to API
      const newUser = new Users;
      newUser.userName = this.createNewEmployee.getRawValue().userName!;
      newUser.userEmail = this.createNewEmployee.getRawValue().userEmail!;
      newUser.phoneNumber = this.createNewEmployee.getRawValue().phoneNumber!;
      newUser.userType = Number.parseInt(this.roleForm.get("userType")!.value);
      newUser.toBeDisabledDate = new Date(this.roleForm.get("toBeDisabledDate")!.value);

      console.log('Request Data:', newUser);
      const observable = this.http.post<Users>(this.apiUrl + "?password=" + this.createNewEmployee.getRawValue().password, newUser);
      const response = await firstValueFrom(observable);
      this.state.user.push(response);
      console.log(observable)
      this.state.sortUser();
      this.modalController.dismiss();
    } catch (e) {
      console.log(e)
      if (e instanceof HttpErrorResponse){
        this.toast.create({message: e.error.messageToClient, duration: 1000}).then((res) => res.present())
      }
    }
  }

    async modalClose(){
    this.modalController.dismiss();
    }

  private sortUserList() {
    console.log('before sort',this.state.currentUser)
    this.state.user = this.state.user.sort((a, b) =>{
      return a.disabled ? 1 : -1;
    });
    console.log('After sort', this.state.user)
  }

  protected readonly UserType = UserType;
}

