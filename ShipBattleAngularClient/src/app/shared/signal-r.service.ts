import * as signalR from "@aspnet/signalr";
import { environment } from "src/environments/environment";
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: "root"
})
export class SignalRService {

  async Start() {
    await this.Connection.start();
  }
  async Stop(){
    await this.Connection.stop();
  }

  public Connection: signalR.HubConnection;

  constructor() {
    this.Connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.AddressHubSR)
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }
}
