import { Injectable } from "@angular/core";
import { SignalRService } from "./signal-r.service";
import { ShipModel } from "../models/ship.model";
import { InfoAct } from "../models/infoAct.model";

@Injectable({
  providedIn: "root"
})
export class GameService {
  constructor(private signalRService: SignalRService) {}

  async start(cmd: string): Promise<boolean> {
    let user = cmd.substring(cmd.indexOf(" ") + 1);
    return await this.signalRService.connection.invoke("start", user);
  }

  async addship(cmd: string): Promise<ShipModel | null> {
    let regex = /addship ([012]) (\d+) (\d+) (\d+) (\d+) (\d+) (\d+) (\d+) ?(\d+)?/;
    let match = regex.exec(cmd);
    if (match !== null) {
      let ship: ShipModel = new ShipModel(
        +match[1],
        +match[2],
        +match[3],
        +match[4],
        +match[5],
        +match[6],
        +match[7],
        +match[8],
        +match[9]
      );
      console.dir(ship);
      let res = await this.signalRService.connection.invoke("addship", ship);

      if (res) return ship;
      return null;
    }
    return null;
  }

  async ready(): Promise<boolean> {
    return await this.signalRService.connection.invoke("ready");
  }

  async move(cmd: string): Promise<InfoAct | null> {
    let regex = /move (\d+) (\d+) (\d+)/;
    let match = regex.exec(cmd);
    if (match !== null) {
      let infoMove = new InfoAct(+match[1], +match[2], +match[3]);
      let res = await this.signalRService.connection.invoke("move", infoMove);

      if (res) return infoMove;
      return null;
    }
    return null;
  }

  async shot(cmd: string): Promise<number> {
    let regex = /shot (\d+) (\d+) (\d+)/;
    let match = regex.exec(cmd);
    if (match !== null) {
      let infoShot = new InfoAct(+match[1], +match[2], +match[3]);
      return await this.signalRService.connection.invoke("shot", infoShot);
    }
    return 0;
  }

  async fix(cmd: string): Promise<InfoAct | null> {
    let regex = /fix (\d+) (\d+) (\d+)/;
    let match = regex.exec(cmd);
    if (match !== null) {
      let infoFix = new InfoAct(+match[1], +match[2], +match[3]);

      let res = await this.signalRService.connection.invoke("fix", infoFix);
      
      if (res) return infoFix;
      return null;
    }
    return null;
  }
}
