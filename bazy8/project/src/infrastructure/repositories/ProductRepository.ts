import { IProductRepository } from "../../domain/repositories/IProductRepository";
import { Product } from "../../domain/entities/Product";
import { InMemoryRepository } from "./InMemoryRepository";
import { Money } from "../../domain/valueobjects/Money";

export class ProductRepository
  extends InMemoryRepository<Product, string>
  implements IProductRepository
{
  async save(product: Product): Promise<Product> {
    const productCopy = new Product(
      product.getId(),
      product.name,
      product.description,
      new Money(product.price.amount, product.price.currency),
      product.stock
    );
    this.storage.set(productCopy.getId(), productCopy);
    return productCopy;
  }

  async findById(id: string): Promise<Product | null> {
    const product = this.storage.get(id);
    if (!product) {
      return null;
    }
    return new Product(
      product.getId(),
      product.name,
      product.description,
      new Money(product.price.amount, product.price.currency),
      product.stock
    );
  }

  async findAll(): Promise<Product[]> {
    return Array.from(this.storage.values()).map(
      (product) =>
        new Product(
          product.getId(),
          product.name,
          product.description,
          new Money(product.price.amount, product.price.currency),
          product.stock
        )
    );
  }

  async findByName(name: string): Promise<Product[]> {
    const products = await this.findAll();
    return products.filter((p) =>
      p.name.toLowerCase().includes(name.toLowerCase())
    );
  }
}
