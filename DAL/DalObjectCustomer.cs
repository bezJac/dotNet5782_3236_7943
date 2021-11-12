using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject:IDal
    {
        /// <summary>
        /// add a customer to customers list in data source layer
        /// </summary>
        /// <param name="person"> customer object to be added </param>
        /// <exception cref = "CustomerExceptionDAL"> thrown if id already exists  </exception>
        public void AddCustomer(Customer person)
        {
            if (DataSource.Customers.Any(customer => (customer.Id == person.Id)))
                throw new CustomerExceptionDAL("id already exists");
            DataSource.Customers.Add(person);
        }

        /// <summary>
        /// update a customer in the list
        /// </summary>
        /// <param name="person"> updated version of customer </param>
        ///  <exception cref = "CustomerExceptionDAL"> thrown if id not founf  </exception>
        public void UpdateCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new CustomerExceptionDAL("id not found");
            DataSource.Customers[index] = person;
        }

        /// <summary>
        /// remove a customer from list
        /// </summary>
        /// <param name="person"> customer to be removed </param>
        /// <exception cref = "CustomerExceptionDAL"> thrown if id not found  </exception> 
        public void RemoveCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new CustomerExceptionDAL("id not found");
            DataSource.Customers.RemoveAt(index);
        }

        /// <summary>
        /// get a copy of a single customer 
        /// </summary>
        /// <param name="id">  customer's ID </param>
        /// <exception cref="DroneExceptionDAL"> thrown if id not founf in list </exception>
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
                throw new CustomerExceptionDAL("id not found");
            }
            return (Customer)temp;
        }

        /// <summary>
        /// get a copy list containing all customers
        /// </summary>
        /// <returns> IEnumerable<Customer> type </returns>
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            if (predicate == null)
                return DataSource.Customers.ToList();
            return DataSource.Customers.FindAll(predicate).ToList();
        }

    }
}
