import { Component, OnInit } from '@angular/core';
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

  private apiUrl = 'http://localhost:5000/api/users';

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
  }

  async updateEmployee(){
    const modal = await this.modalController.create({
      component: EmployeeUpdateComponent
    })
    modal.present();
  }

}
