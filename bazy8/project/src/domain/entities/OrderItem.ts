import { Money } from "../valueobjects/Money";

export class OrderItem {
  constructor(
    public readonly productId: string,
    public readonly productName: string,
    public readonly quantity: number,
    public readonly unitPrice: Money
  ) {}

  getTotalPrice(): Money {
    return new Money(
      this.unitPrice.amount * this.quantity,
      this.unitPrice.currency
    );
  }
}
