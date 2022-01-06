using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer person)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (customers.Any(customer => (customer.Id == person.Id)))
                throw new ExsistException($"id number {person.Id} already exists");
            customers.Add(person);
            XMLTools.SaveListToXMLSerializer<Customer>(customers, customerPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer person)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            int index = customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            customers[index] = person;
            XMLTools.SaveListToXMLSerializer<Customer>(customers, customerPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(Customer person)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            int index = customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            XMLTools.SaveListToXMLSerializer<Customer>(customers, customerPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int id)
        {
            IEnumerable<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer? temp = null;

            temp = (from cs in customers
                    where cs.Id == id
                    select cs).FirstOrDefault();

            if (temp.Value.Id == 0)
                throw new NonExistsException($"ID number {id} not found");

            return (Customer)temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomers(Func<Customer, bool> predicate = null)
        {
            IEnumerable<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (predicate == null)
            {
                if (!customers.Any())
                    throw new NonExistsException("no customers in list");
                return customers;
            }
            customers = customers.Where(predicate);
            if (customers.Any())
                return customers;
            else
                throw new NonExistsException("No Customers in list match predicate");
        }
    }
}
