import {Component, OnInit} from '@angular/core';
import {State} from "../../state";
import {Users} from "../../models";
import {HttpClient} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {catchError} from 'rxjs/operators'

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent  implements OnInit {
  users: Users[] = [];
  apiUrl = 'http://localhost:5000/api/users';

  constructor(public state: State, public http: HttpClient) { }

  ngOnInit() {
    this.fetchUserData().subscribe((userData) => {
      if(userData !== undefined){
        this.state.user = userData;

        this.sortUserList()
      }
    });
  }

  /**
   * A method used to focus an employee on the list
   * @param user
   */
  focusEmployee(user: Users){
    this.state.selectedUser = user;
    console.log('Focus Employee', this.state.selectedUser)
  }

  /**
   * A method used to sort users. Puts disabled users at the bottom of the list.
   * @private
   */
  private sortUserList() {
    console.log('before sort',this.state.currentUser)
    this.state.user = this.state.user.sort((a) =>{
      return a.disabled ? 1 : -1;
    });
    console.log('After sort', this.state.user)
  }

  /**
   * A method used to fetch an array of users.
   */
  fetchUserData(): Observable<Users[] | undefined> {
      return this.http.get<Users[]>(this.apiUrl).pipe(
        catchError((error) => {
          console.error('Eror fetching user data', error);
          return of(undefined);
        })
      );
    }
}
