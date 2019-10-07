import { Component, OnInit } from "@angular/core";
import { SignalRService } from "../shared/signal-r.service";
import { GameService } from "../shared/game.service";
import { ShipModel } from "../models/ship.model";
import { InfoAct } from "../models/infoAct.model";

@Component({
  selector: "app-game",
  templateUrl: "./game.component.html",
  styleUrls: ["./game.component.css"]
})
export class GameComponent {
  constructor(
    private signalRService: SignalRService,
    private gameService: GameService
  ) {
    this.signalRService.connection.on("ReceiveMessage", this.receiveMessage.bind(this));
    this.signalRService.connection.on("PrepareGame", this.prepareGame.bind(this));
  }

  inputText = '';
  inputCmd = "";
  disabledCmd = false;

  receiveMessage(msg: string, error: boolean) {
    if (error) {
      this.disabledCmd = true;
    }
    this.writeLine(msg);
  }

  prepareGame(id: number, enemy: string) {
    this.writeLine(
      `You are playing with ${enemy}. GameFieldId is ${id}. Add ships on your field!`
    );
  }

  writeLine(text: string) {
    this.write(text + "\n");
  }

  write(text: string) {
    this.inputText += text;
  }

  start(cmd: string) {
    this.gameService.start(cmd).then(res => {
      if (res) this.writeLine("Offer sent!");
    });
  }

  OnSubmit() {
    if (this.inputCmd.length > 0) {
      let cmd = this.inputCmd;
      this.inputCmd = "";
      this.writeLine(cmd);

      if (cmd.startsWith("start")) {
        this.start(cmd);
      } else if (cmd.startsWith("addship")) {
        this.addship(cmd);
      } else if (cmd.startsWith("ready")) {
        this.ready();
      } else if (cmd.startsWith("move")) {
        this.move(cmd);
      } else if (cmd.startsWith("shot")) {
        this.shot(cmd);
      } else if (cmd.startsWith("fix")) {
        this.fix(cmd);
      }
    }
  }

  fix(cmd: string) {
    this.gameService.fix(cmd).then((res: InfoAct | null) => {
      if (res !== null) this.writeLine(`Ship ${res.Num} fixed ship!`);
      else this.writeLine("Error!");
    });
  }

  shot(cmd: string) {
    this.gameService.shot(cmd).then((res: number) => {
      if (res == 2) this.writeLine(`Ship killed another ship!`);
      else if (res == 1) this.writeLine(`Ship hitted another ship!`);
      else this.writeLine(`Ship missed another ship!`);
    });
  }

  move(cmd: string) {
    this.gameService.move(cmd).then((res: InfoAct | null) => {
      if (res !== null)
        this.writeLine(`Ship ${res.Num} moved on  (${res.X}:${res.Y})`);
      else this.writeLine("Error!");
    });
  }

  ready() {
    this.writeLine("Waitting...");
    this.gameService.ready().then(res => {
      if (res) this.writeLine("You are ready!");
      else this.writeLine("Can be you did not add ships!");
    });
  }

  addship(cmd: string) {
    this.gameService.addship(cmd).then((res: ShipModel | null) => {
      if (res !== null)
        this.writeLine(
          `Added new ship ${res.TypeShip} (${res.X}:${res.Y}:${res.Len})`
        );
      else this.writeLine("Error!");
    });
  }
}
