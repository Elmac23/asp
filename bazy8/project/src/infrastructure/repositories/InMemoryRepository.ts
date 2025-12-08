import { IRepository } from "../../domain/repositories/IRepository";
import { Entity } from "../../domain/entities/Entity";

export abstract class InMemoryRepository<T extends Entity<ID>, ID>
  implements IRepository<T, ID>
{
  protected storage: Map<ID, T> = new Map();

  async findById(id: ID): Promise<T | null> {
    return this.storage.get(id) || null;
  }

  async findAll(): Promise<T[]> {
    return Array.from(this.storage.values());
  }

  async save(entity: T): Promise<T> {
    this.storage.set(entity.getId(), entity);
    return entity;
  }

  async delete(id: ID): Promise<void> {
    this.storage.delete(id);
  }
}
