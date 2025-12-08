import { IProductRepository } from "../../domain/repositories/IProductRepository";
import { Product } from "../../domain/entities/Product";
import { InMemoryRepository } from "./InMemoryRepository";

export class ProductRepository
  extends InMemoryRepository<Product, string>
  implements IProductRepository
{
  async findByName(name: string): Promise<Product[]> {
    const products = await this.findAll();
    return products.filter((p) =>
      p.name.toLowerCase().includes(name.toLowerCase())
    );
  }
}
