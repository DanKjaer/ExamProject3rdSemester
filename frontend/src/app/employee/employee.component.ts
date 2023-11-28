import { Component, OnInit } from '@angular/core';
import {ModalController} from "@ionic/angular";
import {EmployeeCreateComponent} from "../employee-create/employee-create.component";
import {EmployeeUpdateComponent} from "../employee-update/employee-update.component";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent  implements OnInit {

  constructor(public modalController: ModalController) { }

  ngOnInit() {}

  async createEmployee(){
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
