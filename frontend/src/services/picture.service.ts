import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import {UserType, Users } from "src/models";
import { State } from "src/state";

export interface UserUpdate {
  userID: number;
  userName: string;
  userEmail: string;
  phoneNumber: string;
  disabled: boolean;
  toBeDisabledDate: Date;
  userType: UserType;
  profilePicture: File | null;
}

@Injectable()
export class PictureService{

  constructor(private readonly http: HttpClient, private readonly state: State) {}

  update(value: UserUpdate) {
    const formData = new FormData();
    Object.entries(value).forEach(([key, value]) =>
      formData.append(key, value)
    );
    if(formData.get("password")){
      return this.http.put<Users>('/api/users/' + this.state.selectedUser.userID.valueOf() + "?" + formData.get("password"), formData);
    }
    return this.http.put<Users>('/api/users/' + this.state.selectedUser.userID.valueOf(), formData);
  }
}
