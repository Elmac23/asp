import { Entity } from "./Entity";
import { OrderItem } from "./OrderItem";
import { Money } from "../valueobjects/Money";

export enum OrderStatus {
  CREATED = "CREATED",
  CONFIRMED = "CONFIRMED",
  PAID = "PAID",
  SHIPPED = "SHIPPED",
  DELIVERED = "DELIVERED",
  CANCELLED = "CANCELLED",
}

export class Order extends Entity<string> {
  private items: OrderItem[] = [];
  private status: OrderStatus = OrderStatus.CREATED;

  constructor(
    id: string,
    public readonly customerId: string,
    public readonly createdAt: Date = new Date()
  ) {
    super(id);
  }

  addItem(item: OrderItem): void {
    if (this.status !== OrderStatus.CREATED) {
      throw new Error("Cannot modify order after confirmation");
    }
    this.items.push(item);
  }

  removeItem(productId: string): void {
    if (this.status !== OrderStatus.CREATED) {
      throw new Error("Cannot modify order after confirmation");
    }
    this.items = this.items.filter((item) => item.productId !== productId);
  }

  getItems(): OrderItem[] {
    return [...this.items];
  }

  getTotalAmount(): Money {
    if (this.items.length === 0) {
      return new Money(0);
    }
    return this.items.reduce(
      (total, item) => total.add(item.getTotalPrice()),
      new Money(0)
    );
  }

  confirm(): void {
    if (this.status !== OrderStatus.CREATED) {
      throw new Error("Order already confirmed");
    }
    if (this.items.length === 0) {
      throw new Error("Cannot confirm empty order");
    }
    this.status = OrderStatus.CONFIRMED;
  }

  pay(): void {
    if (this.status !== OrderStatus.CONFIRMED) {
      throw new Error("Order must be confirmed before payment");
    }
    this.status = OrderStatus.PAID;
  }

  ship(): void {
    if (this.status !== OrderStatus.PAID) {
      throw new Error("Order must be paid before shipping");
    }
    this.status = OrderStatus.SHIPPED;
  }

  deliver(): void {
    if (this.status !== OrderStatus.SHIPPED) {
      throw new Error("Order must be shipped before delivery");
    }
    this.status = OrderStatus.DELIVERED;
  }

  cancel(): void {
    if (this.status === OrderStatus.DELIVERED) {
      throw new Error("Cannot cancel delivered order");
    }
    this.status = OrderStatus.CANCELLED;
  }

  getStatus(): OrderStatus {
    return this.status;
  }
}
