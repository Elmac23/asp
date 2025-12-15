import { IProductRepository } from "../../domain/repositories/IProductRepository";
import { Product } from "../../domain/entities/Product";
import { Money } from "../../domain/valueobjects/Money";

export class ProductService {
  constructor(private productRepository: IProductRepository) {}

  async getAllProducts(): Promise<Product[]> {
    return this.productRepository.findAll();
  }

  async getProductById(id: string): Promise<Product | null> {
    return this.productRepository.findById(id);
  }

  async searchProducts(name: string): Promise<Product[]> {
    return this.productRepository.findByName(name);
  }

  async createProduct(
    id: string,
    name: string,
    description: string,
    price: number,
    stock: number
  ): Promise<Product> {
    const product = new Product(id, name, description, new Money(price), stock);
    return this.productRepository.save(product);
  }

  async updateStock(id: string, quantity: number): Promise<void> {
    const product = await this.productRepository.findById(id);
    if (!product) {
      throw new Error("Product not found");
    }
    product.increaseStock(quantity);
    await this.productRepository.save(product);
  }

  async updateProduct(
    id: string,
    name: string,
    description: string,
    price: number,
    stock: number
  ): Promise<Product> {
    const existingProduct = await this.productRepository.findById(id);
    if (!existingProduct) {
      throw new Error("Product not found");
    }
    const updatedProduct = new Product(
      id,
      name,
      description,
      new Money(price),
      stock
    );
    return this.productRepository.save(updatedProduct);
  }

  async deleteProduct(id: string): Promise<void> {
    const product = await this.productRepository.findById(id);
    if (!product) {
      throw new Error("Product not found");
    }
    await this.productRepository.delete(id);
  }
}
