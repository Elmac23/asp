import { Entity } from "./Entity";
import { Money } from "../valueobjects/Money";

export class Product extends Entity<string> {
  constructor(
    id: string,
    public name: string,
    public description: string,
    public price: Money,
    public stock: number
  ) {
    super(id);
  }

  decreaseStock(quantity: number): void {
    if (this.stock < quantity) {
      throw new Error("Insufficient stock");
    }
    this.stock -= quantity;
  }

  increaseStock(quantity: number): void {
    this.stock += quantity;
  }
}
