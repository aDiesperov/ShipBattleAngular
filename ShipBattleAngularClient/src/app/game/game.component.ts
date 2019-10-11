import { Component, OnInit, OnDestroy } from "@angular/core";
import { SignalRService } from "../shared/signal-r.service";
import { GameService } from "../shared/game.service";
import { ShipModel } from "../models/ship.model";
import { InfoAct } from "../models/infoAct.model";
import { CanvasGameField } from 'src/assets/graphic-framework/CanvasGameField';

@Component({
  selector: "app-game",
  templateUrl: "./game.component.html",
  styleUrls: ["./game.component.css"]
})
export class GameComponent implements OnInit, OnDestroy {

  async ngOnDestroy(): Promise<void> {
    await this.signalRService.Stop();
  }

  _canvasCtx: CanvasRenderingContext2D;
  _widthCanvas: number;
  _heightCanvas: number;

  ngOnInit(): void {
    let canvasEl = document.getElementById("myCanvas") as HTMLCanvasElement;
    this._canvasCtx = canvasEl.getContext("2d");
    this._widthCanvas = canvasEl.width;
    this._heightCanvas = canvasEl.height;
  }

  constructor(
    private signalRService: SignalRService,
    private gameService: GameService
  ) {
    this.signalRService.Connection.on("ReceiveMessage", this.receiveMessage.bind(this));
    this.signalRService.Connection.on("PrepareGame", this.prepareGame.bind(this));
    this.signalRService.Connection.on("Hit", this.hit.bind(this));
    this.signalRService.Connection.on("Fix", this.fixed.bind(this));
    this.signalRService.Connection.on("Offer", this.offer.bind(this));
    this.signalRService.Connection.on("NextStep", this.myNextStep.bind(this));

    this.signalRService.Start().then(() => console.log('Connected!'));
  }

  inputText = "";
  inputCmd = "";
  disabledCmd = false;

  fixed(num: number, broken: number) {
    this.gameService.fixed(num, broken);
    this.drawShips();
  }

  hit(msg: string, num: number, damage: number, died: boolean) {
    this.writeLine(msg);
    this.gameService.hit(num, damage, died);
    this.drawShips();
  }

  receiveMessage(msg: string, error: boolean) {
    if (error) {
      this.disabledCmd = true;
    }
    this.writeLine(msg);
  }

  prepareGame(id: number, enemy: string) {
    this.gameService.ships = [];
    this.drawShips();
    this.writeLine(
      `You are playing with ${enemy}. GameFieldId is ${id}. Add ships on your field!`
    );
  }

  offer(name: string) {
    this.writeLine(`User ${name} want to play with you!`);
  }

  myNextStep() {
    this.writeLine(`Your turn!`);
  }

  writeLine(text: string) {
    this.write(text + "\n");
  }

  write(text: string) {
    this.inputText += text;
  }

  public OnSubmit() {
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
      } else if (cmd.startsWith('nextStep')) {
        this.nextStep();
      }
    }
  }

  private start(cmd: string) {
    this.gameService.start(cmd).then(res => {
      if (res) this.writeLine("Offer sent!");
    });
  }

  private fix(cmd: string) {
    this.gameService.fix(cmd).then((res: InfoAct | null) => {
      if (res !== null) this.writeLine(`Ship ${res.Num} fixed ship!`);
      else this.writeLine("Error!");
    });
  }

  private shot(cmd: string) {
    this.gameService.shot(cmd).then((res: number) => {
      switch (res) {
        case 3:
          break;
        case 2:
          this.writeLine(`Ship killed another ship!`);
          break;
        case 1:
          this.writeLine(`Ship hitted another ship!`);
          break;
        default:
          this.writeLine(`Ship missed another ship!`);
          break;
      }
    });
  }

  private move(cmd: string) {
    this.gameService.move(cmd).then((res: InfoAct | null) => {
      if (res !== null) {
        this.writeLine(`Ship ${res.Num} moved on  (${res.X}:${res.Y})`);
        this.drawShips();
      } else this.writeLine("Error!");
    });
  }

  private ready() {
    this.writeLine("Waitting...");
    this.gameService.ready().then(res => {
      switch (res) {
        case 2:
          break;
        case 1:
            this.writeLine("You are ready!");
          break;
        default:
            this.writeLine("Can be you did not add ships!");
          break;
      }
    });
  }

  private addship(cmd: string) {
    this.gameService.addship(cmd).then((res: ShipModel | null) => {
      if (res !== null) {
        this.writeLine(`Added new ship ${res.TypeShip} (${res.X}:${res.Y}:${res.Len})`);
        this.drawShips();
      } else this.writeLine("Error!");
    });
  }

  private nextStep() {
    this.writeLine('Waitting...');
    this.gameService.nextStep().then(
      (res: boolean) => {
        if (!res) this.writeLine('error!');
      }
    );
  }

  drawShips() {
    let canvasGameField = new CanvasGameField();
    this.gameService.ships.forEach(ship => canvasGameField.AddShip(ship.TypeShip, ship.X, ship.Y, ship.Dir, ship.Len, ship.Broken))
    canvasGameField.Draw(this._canvasCtx, this._widthCanvas, this._heightCanvas);
  }
}
