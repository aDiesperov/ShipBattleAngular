import { TypeShips } from './typeShips.enum';

export class ShipGraphic {
    Type: TypeShips;
    X: number;
    Y: number;
    Dir: number;
    Len: number;
    Broken: number;

    constructor(type: TypeShips, x: number, y: number, dir: number, len: number, broken: number) {
        this.Type = type;
        this.X = x;
        this.Y = y;
        this.Dir = dir;
        this.Len = len;
        this.Broken = broken;    
    }
}