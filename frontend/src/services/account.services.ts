import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";

@Injectable()
export class AccountService {
  constructor(private readonly http: HttpClient) { }

  getCurrentUser() {
    return this.http.get<User>('/api/account/whoami');
  }

  login(value: Credentials) {
    return this.http.post<{ token: string }>('/api/account/login', value);
  }

  register(value: Registration) {
    return this.http.post<any>('/api/account/register', value);
  }
}
