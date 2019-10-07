import { TypeShips } from "./typeShips.enum";

export class ShipModel {
  TypeShip: TypeShips;
  X: number;
  Y: number;
  Dir: number;
  Len: number;
  Speed: number;
  R_act: number;
  Act1: number;
  Act2: number;

  /**
   *
   */
  constructor(
    typeShip: TypeShips,
    x: number,
    y: number,
    dir: number,
    len: number,
    speed: number,
    r_act: number,
    act1: number,
    act2: number
  ) {
    this.TypeShip = typeShip;
    this.X = x;
    this.Y = y;
    this.Dir = dir;
    this.Len = len;
    this.Speed = speed;
    this.R_act = r_act;
    this.Act1 = act1;
    this.Act2 = isNaN(act2) ? 0 : act2;
  }
}
