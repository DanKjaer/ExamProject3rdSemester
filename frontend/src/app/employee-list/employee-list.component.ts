import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {State} from "../../state";
import {Users} from "../../models";

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent  implements OnInit {


  constructor(public state: State) { }

  ngOnInit() {}

  focusEmployee(user: Users){
    this.state.selectedUser = user;
    console.log('Focus Employee', this.state.selectedUser)
  }

}
 