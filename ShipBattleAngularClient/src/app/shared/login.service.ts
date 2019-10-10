import { Injectable } from "@angular/core";
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: "root"
})
export class LoginService {
  
  private readonly _loginName = "name";

  constructor(private cookieService: CookieService) {}

  Login(name: string): boolean {
    if(this.IsAuthenticated) return false;
    this.cookieService.set(this._loginName, name);
    return true;
  }

  Logout(): void {
    this.cookieService.delete(this._loginName);
  }

  get IsAuthenticated(): boolean {
    return this.cookieService.check(this._loginName);
  }

  get Name(): string {
    return this.cookieService.get(this._loginName);
  }
}
