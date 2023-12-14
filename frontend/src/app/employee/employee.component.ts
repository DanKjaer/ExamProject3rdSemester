import {Component, OnInit} from '@angular/core';
import {ModalController} from "@ionic/angular";
import {EmployeeCreateComponent} from "../employee-create/employee-create.component";
import {EmployeeUpdateComponent} from "../employee-update/employee-update.component";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Users, UserType} from "../../models";
import {State} from "../../state";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent  implements OnInit {
  apiUrl = 'http://localhost:5000/api/users';



  constructor(public modalController: ModalController, public http: HttpClient, public state: State) { }

  ngOnInit() {
    this.getEmployeeList();
  }

  async getEmployeeList(){
    try{
      const result = await firstValueFrom(this.http.get<Users[]>(this.apiUrl))
      this.state.user = result!;
    }catch(error){
      console.error('Error fetching data:', error)
    }

  }

  /**
   * Modal window for create employee
   */
  async openCreateEmployeeModal(){
    const modal = await this.modalController.create({
      component: EmployeeCreateComponent
    })
    modal.present();
  }

  /**
   * Modal window for the update employee
   */
  async updateEmployee(){
    const modal = await this.modalController.create({
      component: EmployeeUpdateComponent
    })
    modal.present();
  }

  /**
   * A method that changes a users status to disabled
   * @param userId
   */
  async disableEmployee(userId: number){
    this.state.selectedUser.disabled = true;
    const url = `${this.apiUrl}/${userId}`
    const updatedUser = this.state.user.find(user => user.userID === userId);
    if(updatedUser !== null && updatedUser !== undefined){
      updatedUser.disabled = true
      await firstValueFrom(this.http.put<Users>(url, this.state.selectedUser));
      this.state.sortUser();
      this.state.selectedUser = new Users();
    }else{
      console.error('User not found for ID: ', userId);
    }
  }
}

