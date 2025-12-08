import { ICustomerRepository } from "../../domain/repositories/ICustomerRepository";
import { Customer } from "../../domain/entities/Customer";
import { Address } from "../../domain/valueobjects/Address";

export class CustomerService {
  constructor(private customerRepository: ICustomerRepository) {}

  async getAllCustomers(): Promise<Customer[]> {
    return this.customerRepository.findAll();
  }

  async getCustomerById(id: string): Promise<Customer | null> {
    return this.customerRepository.findById(id);
  }

  async getCustomerByEmail(email: string): Promise<Customer | null> {
    return this.customerRepository.findByEmail(email);
  }

  async createCustomer(
    id: string,
    name: string,
    email: string,
    street: string,
    city: string,
    postalCode: string,
    country: string
  ): Promise<Customer> {
    const address = new Address(street, city, postalCode, country);
    const customer = new Customer(id, name, email, address);
    return this.customerRepository.save(customer);
  }

  async updateCustomerAddress(
    id: string,
    street: string,
    city: string,
    postalCode: string,
    country: string
  ): Promise<void> {
    const customer = await this.customerRepository.findById(id);
    if (!customer) {
      throw new Error("Customer not found");
    }
    const newAddress = new Address(street, city, postalCode, country);
    customer.updateAddress(newAddress);
    await this.customerRepository.save(customer);
  }
}
