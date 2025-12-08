import { Entity } from "./Entity";
import { Address } from "../valueobjects/Address";

export class Customer extends Entity<string> {
  constructor(
    id: string,
    public name: string,
    public email: string,
    public address: Address
  ) {
    super(id);
  }

  updateAddress(address: Address): void {
    this.address = address;
  }
}
