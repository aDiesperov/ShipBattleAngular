import { TypeShips } from './typeShips.enum';
import { ShipGraphic } from './ShipGraphic';

export class CanvasGameField {

    private _ships: ShipGraphic[] = [];
    private _ctx: CanvasRenderingContext2D;

    private originWidth = 0;
    private originHeight = 0;

    private width = 0;
    private height = 0;

    public AddShip(type: TypeShips, x: number, y: number, dir: number, len: number, damage: number): void {
        this._ships.push(new ShipGraphic(type, x, y, dir, len, damage));
    }

    public Clear() {
        this._ships = [];
    }

    public Draw(ctx: CanvasRenderingContext2D, width: number, height: number): void {
        this._ctx = ctx;
        ctx.lineWidth = 4;

        this.originWidth = width;
        this.originHeight = height;
        this.width = this.GetWidth();
        this.height = this.GetHeight();

        this.drawBase();

        this._ships.forEach(ship => {
            this.drawShip(ship);
        });
    }

    private drawBase(): void {
        this._ctx.clearRect(0, 0, this.originWidth, this.originHeight);
        this._ctx.strokeStyle = "#000000";
        this._ctx.beginPath();
        this._ctx.moveTo(0, this.originHeight / 2);
        this._ctx.lineTo(this.originWidth, this.originHeight / 2);
        this._ctx.moveTo(this.originWidth / 2, 0);
        this._ctx.lineTo(this.originWidth / 2, this.originHeight);
        this._ctx.closePath();
        this._ctx.stroke();
    }

    private GetWidth(): number {
        let max = 0;
        this._ships.forEach(element => {
            if (Math.abs(element.X) + element.Len + 2 > max) max = Math.abs(element.X) + element.Len + 2;
        });
        return max * 2;
    }

    private GetHeight(): number {
        let max = 0;
        this._ships.forEach(element => {
            if (Math.abs(element.Y) + element.Len + 2 > max) max = Math.abs(element.Y) + element.Len + 2;
        });
        return max * 2;
    }

    private drawShip(ship: ShipGraphic): void {

        this._ctx.fillStyle = '#00FF00';
        this._ctx.strokeStyle = (ship.Type == TypeShips.Military) ? '#D2691E' : (ship.Type == TypeShips.Support) ? '#20B2AA' : '#4B0082';

        let zeroH = this.originHeight / 2;
        let zeroW = this.originWidth / 2;
        let relativeX = this.originHeight / this.height;
        let relativeY = this.originWidth / this.width;
        let width = relativeX;
        let height = relativeY;

        if (ship.Dir)  height *= ship.Len;
        else  width *= ship.Len;

        this._ctx.fillRect(zeroW + ship.X * relativeX, zeroH - ship.Y * relativeY, width, height);

        if (ship.Broken > 0) {
            this._ctx.fillStyle = '#FF0000';

            let widthDamage = relativeX;
            let heightDamage = relativeY;
            if(ship.Dir) heightDamage *= ship.Broken;
            else widthDamage *= ship.Broken;

            this._ctx.fillRect(zeroW + ship.X * relativeX, zeroH - ship.Y * relativeY, widthDamage, heightDamage);
        }
        this._ctx.strokeRect(zeroW + ship.X * relativeX, zeroH - ship.Y * relativeY, width, height);

    }

}