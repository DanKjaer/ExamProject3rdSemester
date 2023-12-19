import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {State} from "../../state";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Users} from "../../models";
import {firstValueFrom} from "rxjs";
import {ModalController, ToastController} from "@ionic/angular";
import {ActivatedRoute} from "@angular/router";
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { PictureService, UserUpdate } from 'src/services/picture.service';

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
    toBeDisabledDate: [this.state.selectedUser.toBeDisabledDate, Validators.required],
    password: [''],
    profilePicture: [null as Blob | null],
  })

  apiUrl = 'https://moonhzoo.azurewebsites.net/api/users';

  constructor(public fb: FormBuilder, public state: State, public http: HttpClient,
              public modalController: ModalController, public toast: ToastController, public route: ActivatedRoute,
              public service: PictureService) { }

  staffId?: string | null;
  UserPicture?: string;
  imageChangedEvent: Event | undefined;

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
      //let dto = this.updateEmployee.getRawValue();
      //const observable = this.http.put<Users>(this.apiUrl + '/' + this.state.selectedUser.userID, dto);
      const response = await firstValueFrom<Users>(this.service.update(this.updateEmployee.value as UserUpdate));
      const index = this.state.user.findIndex((user) => user.userID === this.state.selectedUser.userID);
      if(index !== -1){
        this.state.user[index] = response;
      }
      this.modalController.dismiss();
      this.state.sortUser();
    }catch (e){
      if(e instanceof HttpErrorResponse){
        this.toast.create({message: e.error.messageToClient, duration: 1000}).then((res) => res.present())
      }
    }
  }

  async modalClose(){
    this.modalController.dismiss();
  }

  onFileChanged($event: Event) {
    this.imageChangedEvent = event;
  }

  imageCropped($event: any) {
    this.updateEmployee.patchValue({ profilePicture: $event.blob! });
  }
}
