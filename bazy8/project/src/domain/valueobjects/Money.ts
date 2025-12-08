import { ValueObject } from "./ValueObject";

export class Money extends ValueObject {
  constructor(
    public readonly amount: number,
    public readonly currency: string = "USD"
  ) {
    super();
  }

  equals(other: ValueObject): boolean {
    if (!(other instanceof Money)) return false;
    return this.amount === other.amount && this.currency === other.currency;
  }

  add(other: Money): Money {
    if (this.currency !== other.currency) {
      throw new Error("Cannot add money with different currencies");
    }
    return new Money(this.amount + other.amount, this.currency);
  }
}
