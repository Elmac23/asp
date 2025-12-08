import { ValueObject } from "./ValueObject";

export class Address extends ValueObject {
  constructor(
    public readonly street: string,
    public readonly city: string,
    public readonly postalCode: string,
    public readonly country: string
  ) {
    super();
  }

  equals(other: ValueObject): boolean {
    if (!(other instanceof Address)) return false;
    return (
      this.street === other.street &&
      this.city === other.city &&
      this.postalCode === other.postalCode &&
      this.country === other.country
    );
  }
}
