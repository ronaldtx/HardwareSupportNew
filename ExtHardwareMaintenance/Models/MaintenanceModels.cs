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
    public class MaintenanceModel
    {
        public IMaintenanceService maintenanceService;
        public IUserService userService;
        public IItemTypeService itemTypeService;
        public IItemService itemService;
        public ICustomerService customerService;

        public MaintenanceModel()
        {
            maintenanceService = new MaintenanceService(new MaintenanceRepository(), new MaintenanceValidator());
            userService = new UserService(new UserRepository(), new UserValidator());
            itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            itemService = new ItemService(new ItemRepository(), new ItemValidator());
            customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
        }
    }
    public class Maintenances 
    {
        public static IEnumerable GetAllMaintenance()
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                MaintenanceModel p = new MaintenanceModel();
                IList<Maintenance> maintenance = p.maintenanceService.GetAll();
                return maintenance;
            }
        }

        internal static String SaveData(FormCollection isi)
        {
            // Maintenance maintenance = new Maintenance();
            MaintenanceModel p = new MaintenanceModel();
            int itemId = Int32.Parse(isi["TxtItemId"]);
            int custId = Int32.Parse(isi["TxtCustomerId"]); 
            int userId = Int32.Parse(isi["TxtUserId"]);
            int Case   = Int32.Parse(isi["TxtCase"]);;
            DateTime reqDate = Convert.ToDateTime(isi["TxtRequestDate"]);
            String Complaint = isi["TxtComplaint"];

            Maintenance MaintenanceNew = p.maintenanceService.CreateObject(itemId, custId, userId, reqDate, 
                                                                            Complaint, Case, p.itemService, 
                                                                            p.itemTypeService, p.userService, 
                                                                            p.customerService);

            /*
            maintenance.ItemId = Int32.Parse(isi["TxtItemId"]);
            maintenance.UserId = Int32.Parse(isi["TxtUserId"]);

            maintenance.CustomerId = Int32.Parse(isi["TxtCustomerId"]);
            maintenance.RequestDate = Convert.ToDateTime(isi["TxtRequestDate"]);
            maintenance.Complaint = isi["TxtComplaint"];
            maintenance.Case = Int32.Parse(isi["TxtCase"]);
            maintenance.IsDiagnosed = false;
            maintenance.IsFinished = false;
            
            ItemType itemType = new ItemType();
            itemType = ItemTypes.GetItemType(Int32.Parse(isi["TxtItemId"]));
            maintenance.ItemTypeId = itemType.Id;

            */


            // Maintenance MaintenanceNew = p.maintenanceService.CreateObject(maintenance, p.itemService, p.itemTypeService, p.userService, p.customerService);
            String err = (MaintenanceNew.Errors.Any()) ? p.maintenanceService.GetValidator().PrintError(MaintenanceNew) : "";
            return err;
        }
    }
}