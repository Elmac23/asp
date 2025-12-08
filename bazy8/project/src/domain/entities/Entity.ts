export abstract class Entity<T> {
  protected readonly id: T;

  constructor(id: T) {
    this.id = id;
  }

  getId(): T {
    return this.id;
  }

  equals(other: Entity<T>): boolean {
    return this.id === other.id;
  }
}
