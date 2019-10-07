import { Component, OnInit } from "@angular/core";
import { LoginService } from "../shared/login.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  public name = "";

  constructor(private loginService: LoginService, private router: Router) {}

  ngOnInit() {
    if (this.loginService.IsAuthenticated) {
      this.router.navigateByUrl("/game");
    }
  }

  public OnLogin() : void {
    if (this.name.length > 2 && this.loginService.Login(this.name)) {
        this.router.navigateByUrl("/game");
    }
  }
}
