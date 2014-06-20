using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data.Context;
using Core.DomainModel;
using System.Data.Entity;
using Service.Service;
using Data.Repository;
using Core.Interface.Repository;
using Core.Interface.Service;
using System.Threading;
using Validation.Validation;
using Ext.Net;

namespace ExtHardwareMaintenance.Models
{
    public class CustomerModel
    {
        public ICustomerService customerService;

        public CustomerModel()
        {
            customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
        }
    }
    public class Customers
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static IEnumerable GetAllCustomer()
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                CustomerModel p = new CustomerModel();
                IList<Customer> Customers = p.customerService.GetAll();
                return Customers;
            }
        }
        public static Customer GetCustomer(int id)
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                CustomerModel p = new CustomerModel();
                Customer Customer = p.customerService.GetObjectById(id);
                return Customer;
            }
        }
        /*
        public static void SaveData(Dictionary<string, string> isi)
        {
            Customer Customer = new Customer();
            Customer.Name = isi["TxtName"];
            Customer.Description = isi["TxtDescription"];
            ExtNetModel p = new ExtNetModel();
            Customer CustomerNew = p.customerService.CreateObject(Customer);
        }
        */
        internal static String SaveData(FormCollection isi)
        {
            Customer Customer = new Customer();
            Customer.Name = isi["TxtName"];
            Customer.Address = isi["TxtAddress"];
            Customer.PIC = isi["TxtPic"];
            Customer.Contact= isi["TxtContact"];
            Customer.Email = isi["TxtEmail"];

            CustomerModel p = new CustomerModel();
            Customer CustomerNew = p.customerService.CreateObject(Customer);
            String err = (CustomerNew.Errors.Any()) ? p.customerService.GetValidator().PrintError(CustomerNew) : "";
            return err;
        }
        internal static String SaveDataEdit(FormCollection isi)
        {
            int id = Convert.ToInt16(isi["TxtId"]);
            CustomerModel p = new CustomerModel();
            Customer Customer = p.customerService.GetObjectById(id);

            Customer.Name = isi["TxtName"];
            Customer.Address = isi["TxtAddress"];
            Customer.PIC = isi["TxtPic"];
            Customer.Contact = isi["TxtContact"];
            Customer.Email = isi["TxtEmail"];

            Customer CustomerNew = p.customerService.UpdateObject(Customer);
            String err = (CustomerNew.Errors.Any()) ? p.customerService.GetValidator().PrintError(CustomerNew) : "";
            return err;
        }

        internal static void DataDelete(int id)
        {
            CustomerModel p = new CustomerModel();
            Customer Customer = p.customerService.GetObjectById(id);

            bool CustomerDeleted = p.customerService.DeleteObject(Customer.Id);
        }

        public static Paging<Customer> CustomersPaging(int start, int limit, string sort, string dir, string filter)
        {
            CustomerModel p = new CustomerModel();
            List<Customer> customers = p.customerService.GetAll().ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                customers.RemoveAll(user => !user.Name.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                customers.Sort(delegate(Customer x, Customer y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > customers.Count)
            {
                limit = customers.Count - start;
            }

            List<Customer> rangePlants = (start < 0 || limit < 0) ? customers : customers.GetRange(start, limit);

            return new Paging<Customer>(rangePlants, customers.Count);
        }
    }
}