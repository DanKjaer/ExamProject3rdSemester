import { Component, OnInit } from '@angular/core';
import {State} from "../../state";

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent  implements OnInit {

  constructor(public state: State) { }

  ngOnInit() {}

}
