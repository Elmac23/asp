import { IRepository } from "./IRepository";
import { Customer } from "../entities/Customer";

export interface ICustomerRepository extends IRepository<Customer, string> {
  findByEmail(email: string): Promise<Customer | null>;
}
