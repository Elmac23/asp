import { ICustomerRepository } from "../../domain/repositories/ICustomerRepository";
import { Customer } from "../../domain/entities/Customer";
import { InMemoryRepository } from "./InMemoryRepository";

export class CustomerRepository
  extends InMemoryRepository<Customer, string>
  implements ICustomerRepository
{
  async findByEmail(email: string): Promise<Customer | null> {
    const customers = await this.findAll();
    return customers.find((c) => c.email === email) || null;
  }
}
