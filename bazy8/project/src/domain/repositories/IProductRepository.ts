import { IRepository } from "./IRepository";
import { Product } from "../entities/Product";

export interface IProductRepository extends IRepository<Product, string> {
  findByName(name: string): Promise<Product[]>;
}
