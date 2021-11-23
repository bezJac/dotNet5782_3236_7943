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
    public partial class DalObject:IDal
    {

        /// <summary>
        /// add a customer to customers list in data source layer
        /// </summary>
        /// <param name="person"> customer object to be added </param>
        /// <exception cref = "ExsistException"> thrown if id already exists  </exception>
        public void AddCustomer(Customer person)
        {
            if (DataSource.Customers.Any(customer => (customer.Id == person.Id)))
                throw new ExsistException($"id number {person.Id} already exists");
            DataSource.Customers.Add(person);
        }

        /// <summary>
        /// update a customer in the list
        /// </summary>
        /// <param name="person"> updated version of customer </param>
        ///  <exception cref = "NonExistsException"> thrown if id not found  </exception>
        public void UpdateCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            DataSource.Customers[index] = person;
        }

        /// <summary>
        /// remove a customer from list
        /// </summary>
        /// <param name="person"> customer to be removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception> 
        public void RemoveCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new NonExistsException($"id number {person.Id} not found");
            DataSource.Customers.RemoveAt(index);
        }

        /// <summary>
        /// get a copy of a single customer 
        /// </summary>
        /// <param name="id">  customer's ID </param>
        /// <exception cref="NonExistsException"> thrown if id not found in list </exception>
        /// <returns> copy of customer matching the id </returns>
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

        /// <summary>
        /// get a copy list containing of customers
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Customer> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Customer> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Customers.Count() <= 0)
                    throw new NonExistsException("no customers in list");
                return DataSource.Customers.ToList();
            }
            List<Customer> tmp = DataSource.Customers.FindAll(predicate).ToList();
            if (tmp.Count() > 0)
                return tmp;
            else
                throw new NonExistsException("No Customers in list match predicate");
        }

    }
}
