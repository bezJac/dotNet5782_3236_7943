using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    /// <summary>
    /// partial DalObject class responsible for CRUD of Customer entities. 
    /// </summary>
    public partial class DalObject : IDal
    {
        public void AddCustomer(Customer person)
        {
            if (DataSource.Customers.Any(customer => (customer.Id == person.Id)))
                throw new ExsistException($"id number {person.Id} already exists");
            DataSource.Customers.Add(person);
        }
        public void UpdateCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            DataSource.Customers[index] = person;
        }
        public void RemoveCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            DataSource.Customers.RemoveAt(index);
        }
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
        public IEnumerable<Customer> GetAllCustomers(Func<Customer,bool> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Customers.Count() <= 0)
                    throw new NonExistsException("no customers in list");
                return DataSource.Customers.ToList();
            }
            IEnumerable<Customer> tmp = DataSource.Customers.Where(predicate);
            if (tmp.Count() > 0)
                return tmp;
            else
                throw new NonExistsException("No Customers in list match predicate");
        }
    }
}
