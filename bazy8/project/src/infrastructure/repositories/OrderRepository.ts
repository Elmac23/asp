import { IOrderRepository } from "../../domain/repositories/IOrderRepository";
import { Order } from "../../domain/entities/Order";
import { InMemoryRepository } from "./InMemoryRepository";

export class OrderRepository
  extends InMemoryRepository<Order, string>
  implements IOrderRepository
{
  async findByCustomerId(customerId: string): Promise<Order[]> {
    const orders = await this.findAll();
    return orders.filter((o) => o.customerId === customerId);
  }
}
