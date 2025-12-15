import { IOrderRepository } from "../../domain/repositories/IOrderRepository";
import { IProductRepository } from "../../domain/repositories/IProductRepository";
import { Order, OrderStatus } from "../../domain/entities/Order";
import { OrderItem } from "../../domain/entities/OrderItem";

export class OrderService {
  constructor(
    private orderRepository: IOrderRepository,
    private productRepository: IProductRepository
  ) {}

  async getAllOrders(): Promise<Order[]> {
    return this.orderRepository.findAll();
  }

  async getOrderById(id: string): Promise<Order | null> {
    return this.orderRepository.findById(id);
  }

  async getOrdersByCustomer(customerId: string): Promise<Order[]> {
    return this.orderRepository.findByCustomerId(customerId);
  }

  async createOrder(id: string, customerId: string): Promise<Order> {
    const order = new Order(id, customerId);
    return this.orderRepository.save(order);
  }

  async addItemToOrder(
    orderId: string,
    productId: string,
    quantity: number
  ): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    const product = await this.productRepository.findById(productId);
    if (!product) {
      throw new Error("Product not found");
    }

    const item = new OrderItem(
      productId,
      product.name,
      quantity,
      product.price
    );
    order.addItem(item);
    await this.orderRepository.save(order);
  }

  async removeItemFromOrder(orderId: string, productId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    order.removeItem(productId);
    await this.orderRepository.save(order);
  }

  async confirmOrder(orderId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    for (const item of order.getItems()) {
      const product = await this.productRepository.findById(item.productId);
      if (!product) {
        throw new Error(`Product ${item.productId} not found`);
      }
      product.decreaseStock(item.quantity);
      await this.productRepository.save(product);
    }

    order.confirm();
    await this.orderRepository.save(order);
  }

  async payOrder(orderId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    order.pay();
    await this.orderRepository.save(order);
  }

  async shipOrder(orderId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    order.ship();
    await this.orderRepository.save(order);
  }

  async deliverOrder(orderId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    order.deliver();
    await this.orderRepository.save(order);
  }

  async cancelOrder(orderId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }

    order.cancel();
    await this.orderRepository.save(order);
  }

  async deleteOrder(orderId: string): Promise<void> {
    const order = await this.orderRepository.findById(orderId);
    if (!order) {
      throw new Error("Order not found");
    }
    await this.orderRepository.delete(orderId);
  }
}
