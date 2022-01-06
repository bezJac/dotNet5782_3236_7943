using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;
using DS;

namespace Dal
{
    internal partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer person)
        {
            if (DataSource.Customers.Any(customer => (customer.Id == person.Id)))
                throw new ExsistException($"id number {person.Id} already exists");
            DataSource.Customers.Add(person);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            DataSource.Customers[index] = person;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            DataSource.Customers.RemoveAt(index);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int id)
        {
            Customer? temp = null;
            foreach (Customer cstmr in DataSource.Customers)
            {
                if (cstmr.Id == id)
                {
                    temp = cstmr;
                    break;
                }

            }
            if (temp == null)
            {
                throw new NonExistsException($"id number {id} not found");
            }
            return (Customer)temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomers(Func<Customer,bool> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Customers.Count <= 0)
                    throw new NonExistsException("no customers in list");
                return DataSource.Customers.ToList();
            }
            IEnumerable<Customer> tmp = DataSource.Customers.Where(predicate);
            if (tmp.Any())
                return tmp;
            else
                throw new NonExistsException("No Customers in list match predicate");
        }
    }
}
