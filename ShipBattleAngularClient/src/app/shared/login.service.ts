import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class LoginService {
  
  private readonly _loginName = "name";

  constructor() {}

  Login(name: string): boolean {
    if(this.IsAuthenticated) return false;
    localStorage.setItem(this._loginName, name);
    return true;
  }

  Logout(): void {
    localStorage.removeItem(this._loginName);
  }

  get IsAuthenticated(): boolean {
    if (localStorage.getItem(this._loginName) !== null) return true;
    return false;
  }

  get Name(): string | null {
    return localStorage.getItem(this._loginName);
  }
}
