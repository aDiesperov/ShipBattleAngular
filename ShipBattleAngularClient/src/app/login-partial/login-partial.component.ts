import { Component, OnInit, DoCheck } from '@angular/core';
import { LoginService } from '../shared/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-partial',
  templateUrl: './login-partial.component.html',
  styleUrls: ['./login-partial.component.css']
})
export class LoginPartialComponent implements DoCheck {

  ngDoCheck(): void {
    this.name = this.loginService.Name;
  }

  public name : string | null;

  constructor(private loginService: LoginService, private router: Router) { }

  OnLogout(){
    this.loginService.Logout();
    this.router.navigateByUrl('/login');
  }

  OnLogin(){
    this.router.navigateByUrl('/login');
  }

}
