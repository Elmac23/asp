import { IRepository } from "./IRepository";
import { Order } from "../entities/Order";

export interface IOrderRepository extends IRepository<Order, string> {
  findByCustomerId(customerId: string): Promise<Order[]>;
}
