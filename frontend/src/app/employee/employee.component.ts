import {Component, OnInit} from '@angular/core';
import {ModalController} from "@ionic/angular";
import {EmployeeCreateComponent} from "../employee-create/employee-create.component";
import {EmployeeUpdateComponent} from "../employee-update/employee-update.component";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Users} from "../../models";
import {State} from "../../state";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent  implements OnInit {

  apiUrl = 'http://localhost:5000/api/users';


  constructor(public modalController: ModalController, public http: HttpClient, public state: State,) { }

  ngOnInit() {
    this.getEmployeeList();
  }

  async getEmployeeList(){
    try{
      const result = await firstValueFrom(this.http.get<Users[]>(this.apiUrl))
      console.log(result);
      this.state.user = result!;
    }catch(error){
      console.error('Error fetching data:', error)
    }

  }

  async openCreateEmployeeModal(){
    const modal = await this.modalController.create({
      component: EmployeeCreateComponent
    })
    modal.present();
    console.log("test 33: " + this.state.selectedUser)
  }

  async updateEmployee(){
    const modal = await this.modalController.create({
      component: EmployeeUpdateComponent
    })
    modal.present();
  }

  async disableEmployee(userId: number) {
    this.state.selectedUser.disabled = true;
    await firstValueFrom(this.http.put<Users>(this.apiUrl, this.state.selectedUser))
    console.log("selected user: ", this.state.selectedUser, this.state.selectedUser.userID)
    console.log('UserId given to method: ', userId);
    this.state.user = this.state.user.filter(user => user.userID != userId);
    this.state.selectedUser = new Users();
  }

  async setDisabledUser(){
    const currentDate = new Date();
    await firstValueFrom(this.http.put<Users>(this.apiUrl, this.state.user))
    for(const user of this.state.user){
      if (user.toBeDisabledDate instanceof Date && currentDate >= user.toBeDisabledDate){
        user.disabled = true;
      }
    }
  }
}

