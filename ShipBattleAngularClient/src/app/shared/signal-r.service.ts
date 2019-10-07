import { Injectable } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { environment } from "src/environments/environment";
import { LoginService } from './login.service';

@Injectable({
  providedIn: "root"
})
export class SignalRService {
  public connection: signalR.HubConnection;

  constructor(private loginService: LoginService) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.AddressHubSR, { accessTokenFactory : () => loginService.Name})
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.start().then(res => console.log('connected!'));

  }
}
